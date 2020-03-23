import services.g.identity_pb2_grpc as g
from client import create_client
from client.client import Client
from db import DBSession, get_user
from db.models.account import ProjectUser
import uuid

from services.g.identity_pb2 import *


def delete_db_user(user: ProjectUser, client: Client):
    for instance in user.instances:
        client.connection.delete_server(name_or_id=instance.id, wait=False)
    for volume in user.volumes:
        client.connection.delete_volume(name_or_id=volume.id, wait=False)


class UserManagement(g.IdentityServicer):
    def AddUser(self, req: AddUserRequest, context) -> AddUserResponse:
        session = DBSession()
        user = ProjectUser(id=req.userProjectAssignmentId, user_id=req.userId, project_id=req.projectId)
        session.add(user)
        session.commit()
        return AddUserResponse()

    def DeleteUser(self, request: DeleteUserRequest, context) -> DeleteUserResponse:
        client = create_client()
        session = DBSession()
        for user in session.query(ProjectUser).filter_by(user_id=request.userId):
            delete_db_user(user, client)
            session.delete(user)
        session.commit()
        return DeleteUserResponse()

    def RemoveUserFromProject(self, request: RemoveUserFromProjectRequest, context) -> RemoveUserFromProjectResponse:
        client = create_client()
        session = DBSession()
        user = get_user(session, request.userId, request.projectId)
        # delete instances and volumes from server
        delete_db_user(user, client)

        # delete user from db
        # db instances and volumes will be cascade deleted.
        session.delete(user)
        session.commit()
        return RemoveUserFromProjectResponse()

    def DeleteProject(self, request, context):
        client = create_client()
        session = DBSession()
        for user in session.query(ProjectUser).filter_by(project_id=request.projectId):
            delete_db_user(user, client)
            session.delete(user)
        session.commit()
        return DeleteProjectResponse()


def add_to_server(server):
    g.add_IdentityServicer_to_server(UserManagement(), server)

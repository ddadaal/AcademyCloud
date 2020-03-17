import services.g.identity_pb2_grpc as g
from db import DBSession, get_user
from db.models.account import User
import uuid

from services.g.identity_pb2 import *


class UserManagement(g.IdentityServicer):
    def AddUser(self, req: AddUserRequest, context) -> AddUserResponse:
        session = DBSession()
        user = User(id=req.userProjectAssignmentId, user_id=req.userId, project_id=req.projectId)
        session.add(user)
        session.commit()
        return AddUserResponse()

    def DeleteUser(self, request: DeleteUserRequest, context) -> DeleteUserResponse:
        session = DBSession()
        user = get_user(request.userId, request.projectId)
        session.delete(user)
        session.commit()
        return DeleteUserResponse()


def add_to_server(server):
    g.add_IdentityServicer_to_server(UserManagement(), server)

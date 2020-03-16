import services.g.identity_pb2_grpc as g
from db import DBSession
from db.models.account import User
import uuid

from services.g.identity_pb2 import AddUserResponse, AddUserRequest


class UserManagement(g.IdentityServicer):
    def AddUser(self, req: AddUserRequest, context) -> AddUserResponse:
        session = DBSession()
        user = User(id=uuid.uuid1())
        session.add(user)
        session.commit()
        return AddUserResponse()

    def DeleteUser(self, request, context):
        return super().DeleteUser(request, context)


def add_to_server(server):
    g.add_IdentityServicer_to_server(UserManagement(), server)

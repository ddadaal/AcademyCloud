import services.g.identity_pb2_grpc as g

class UserManagement(g.IdentityServicer):
    def AddUser(self, req, context):
        pass
    def DeleteUser(self, request, context):
        return super().DeleteUser(request, context)

def add_to_server(server):
    g.add_IdentityServicer_to_server(UserManagement(), server)


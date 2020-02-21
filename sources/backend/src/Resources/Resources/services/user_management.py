import services.g.expenses_usermanagement_pb2_grpc as g

class UserManagement(g.UserManagementServicer):
    def AddUser(self, req, context):
        pass

def add_to_server(server):
    g.add_UserManagementServicer_to_server(UserManagement(), server)


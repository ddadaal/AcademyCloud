# Generated by the gRPC Python protocol compiler plugin. DO NOT EDIT!
import grpc

from . import identity_pb2 as identity__pb2


class IdentityStub(object):
  # missing associated documentation comment in .proto file
  pass

  def __init__(self, channel):
    """Constructor.

    Args:
      channel: A grpc.Channel.
    """
    self.AddUser = channel.unary_unary(
        '/identity.Identity/AddUser',
        request_serializer=identity__pb2.AddUserRequest.SerializeToString,
        response_deserializer=identity__pb2.AddUserResponse.FromString,
        )
    self.RemoveUserFromProject = channel.unary_unary(
        '/identity.Identity/RemoveUserFromProject',
        request_serializer=identity__pb2.RemoveUserFromProjectRequest.SerializeToString,
        response_deserializer=identity__pb2.RemoveUserFromProjectResponse.FromString,
        )
    self.DeleteProject = channel.unary_unary(
        '/identity.Identity/DeleteProject',
        request_serializer=identity__pb2.DeleteProjectRequest.SerializeToString,
        response_deserializer=identity__pb2.DeleteProjectResponse.FromString,
        )
    self.DeleteUser = channel.unary_unary(
        '/identity.Identity/DeleteUser',
        request_serializer=identity__pb2.DeleteUserRequest.SerializeToString,
        response_deserializer=identity__pb2.DeleteUserResponse.FromString,
        )


class IdentityServicer(object):
  # missing associated documentation comment in .proto file
  pass

  def AddUser(self, request, context):
    # missing associated documentation comment in .proto file
    pass
    context.set_code(grpc.StatusCode.UNIMPLEMENTED)
    context.set_details('Method not implemented!')
    raise NotImplementedError('Method not implemented!')

  def RemoveUserFromProject(self, request, context):
    # missing associated documentation comment in .proto file
    pass
    context.set_code(grpc.StatusCode.UNIMPLEMENTED)
    context.set_details('Method not implemented!')
    raise NotImplementedError('Method not implemented!')

  def DeleteProject(self, request, context):
    # missing associated documentation comment in .proto file
    pass
    context.set_code(grpc.StatusCode.UNIMPLEMENTED)
    context.set_details('Method not implemented!')
    raise NotImplementedError('Method not implemented!')

  def DeleteUser(self, request, context):
    # missing associated documentation comment in .proto file
    pass
    context.set_code(grpc.StatusCode.UNIMPLEMENTED)
    context.set_details('Method not implemented!')
    raise NotImplementedError('Method not implemented!')


def add_IdentityServicer_to_server(servicer, server):
  rpc_method_handlers = {
      'AddUser': grpc.unary_unary_rpc_method_handler(
          servicer.AddUser,
          request_deserializer=identity__pb2.AddUserRequest.FromString,
          response_serializer=identity__pb2.AddUserResponse.SerializeToString,
      ),
      'RemoveUserFromProject': grpc.unary_unary_rpc_method_handler(
          servicer.RemoveUserFromProject,
          request_deserializer=identity__pb2.RemoveUserFromProjectRequest.FromString,
          response_serializer=identity__pb2.RemoveUserFromProjectResponse.SerializeToString,
      ),
      'DeleteProject': grpc.unary_unary_rpc_method_handler(
          servicer.DeleteProject,
          request_deserializer=identity__pb2.DeleteProjectRequest.FromString,
          response_serializer=identity__pb2.DeleteProjectResponse.SerializeToString,
      ),
      'DeleteUser': grpc.unary_unary_rpc_method_handler(
          servicer.DeleteUser,
          request_deserializer=identity__pb2.DeleteUserRequest.FromString,
          response_serializer=identity__pb2.DeleteUserResponse.SerializeToString,
      ),
  }
  generic_handler = grpc.method_handlers_generic_handler(
      'identity.Identity', rpc_method_handlers)
  server.add_generic_rpc_handlers((generic_handler,))
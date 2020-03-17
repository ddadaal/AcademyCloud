from grpc import ServicerContext

import services.g.instance_pb2_grpc as g
from db import DBSession
import uuid

from services.g.instance_pb2 import *
from utils.decorator import auth_required
from utils.token_claims import TokenClaims


class InstanceManagement(g.InstanceServiceServicer):
    @auth_required
    def GetInstances(self, request: GetInstancesRequest, context: ServicerContext,
                     claims: TokenClaims) -> GetInstancesResponse:
        print(claims)
        resp = GetInstancesResponse()
        return resp

    def CreateInstance(self, request: CreateInstanceRequest, context) -> CreateInstanceResponse:
        pass

    def GetFlavors(self, request: GetFlavorsRequest, context) -> GetFlavorsResponse:
        pass

    def GetImages(self, request: GetImagesRequest, context) -> GetImagesResponse:
        pass


def add_to_server(server):
    g.add_InstanceServiceServicer_to_server(InstanceManagement(), server)

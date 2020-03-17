from grpc import ServicerContext

import services.g.instance_pb2_grpc as g
from client import create_client
from db import DBSession
import uuid

from db.models.account import User
from services.g.instance_pb2 import *
from utils.decorator import auth_required
from utils.token_claims import TokenClaims


class InstanceManagement(g.InstanceServiceServicer):
    @auth_required
    def GetInstances(self, request: GetInstancesRequest, context: ServicerContext,
                     claims: TokenClaims) -> GetInstancesResponse:
        session = DBSession()
        resp = GetInstancesResponse()
        return resp

    def CreateInstance(self, request: CreateInstanceRequest, context) -> CreateInstanceResponse:
        pass

    def GetFlavors(self, request: GetFlavorsRequest, context) -> GetFlavorsResponse:
        client = create_client()
        flavors = client.connection.list_flavors()

        resp = GetFlavorsResponse()
        for flavor in flavors:
            f = Flavor()
            f.id = flavor.id
            f.name = flavor.name
            f.cpu = flavor.vcpus
            f.memory = flavor.ram
            f.rootDisk = flavor.disk
            resp.flavors.append(f)
        return resp

    def GetImages(self, request: GetImagesRequest, context) -> GetImagesResponse:
        client = create_client()
        images = client.connection.list_images()

        resp = GetImagesResponse()
        for image in images:
            i = Image()
            i.id = image.id
            i.name = image.name
            i.minDisk = image.min_disk
            resp.images.append(i)
        return resp


def add_to_server(server):
    g.add_InstanceServiceServicer_to_server(InstanceManagement(), server)

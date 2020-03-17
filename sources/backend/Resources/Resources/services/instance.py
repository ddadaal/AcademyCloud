from datetime import datetime

from grpc import ServicerContext

import services.g.instance_pb2_grpc as g
from client import create_client
from db import DBSession
import uuid

from db.models.account import User
from services.g.instance_pb2 import *
from utils.decorator import auth_required
from utils.token_claims import TokenClaims
from google.protobuf.timestamp_pb2 import Timestamp


class InstanceManagement(g.InstanceServiceServicer):
    @auth_required
    def GetInstances(self, request: GetInstancesRequest, context: ServicerContext,
                     claims: TokenClaims) -> GetInstancesResponse:
        session = DBSession()
        user = session.query(User).filter_by(user_id=claims["UserId"], project_id=claims["ProjectId"]).one()
        instance_ids = [str(x.id) for x in user.instances]

        # find the instances
        client = create_client()
        all_servers = client.connection.list_servers(all_projects=True)

        resp = GetInstancesResponse()
        for server in all_servers:
            if server.id not in instance_ids:
                continue
            i = Instance()
            i.id = server.id
            i.name = server.name
            i.flavor.name = server.flavor.original_name
            i.flavor.cpu = server.flavor.vcpus
            i.flavor.memory = server.flavor.ram
            i.flavor.rootDisk = server.flavor.disk
            i.status = InstanceStatus.Value(server.status)
            i.ip = server.public_v4
            i.createTime = server.created_at
            resp.instances.append(i)

        return resp

    def CreateInstance(self, request: CreateInstanceRequest, context) -> CreateInstanceResponse:
        pass

    def GetFlavors(self, request: GetFlavorsRequest, context) -> GetFlavorsResponse:
        client = create_client()
        flavors = client.connection.list_flavors()

        resp = GetFlavorsResponse()
        for flavor in flavors:
            f = Flavor()
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

from datetime import datetime

from grpc import ServicerContext

import services.g.instance_pb2_grpc as g
from client import create_client
from db import DBSession, get_user_from_claims
import uuid

from db.models.account import User
from db import models
from services.g.instance_pb2 import *
from utils.decorator import auth_required
from utils.token_claims import TokenClaims
from google.protobuf.timestamp_pb2 import Timestamp


class InstanceManagement(g.InstanceServiceServicer):
    @auth_required
    def GetInstances(self, request: GetInstancesRequest, context: ServicerContext,
                     claims: TokenClaims) -> GetInstancesResponse:
        session = DBSession()
        user = get_user_from_claims(session, claims)
        instances = user.instances

        # find the instances
        client = create_client()
        all_servers = client.connection.list_servers(all_projects=True)

        resp = GetInstancesResponse()
        for server in all_servers:
            db_server = next((i for i in instances if str(i.id) == server.id), None)
            if db_server is None:
                continue
            i = Instance()
            i.id = server.id
            i.name = server.name
            i.imageName = db_server.image_name
            i.flavor.name = server.flavor.original_name
            i.flavor.cpu = server.flavor.vcpus
            i.flavor.memory = server.flavor.ram
            i.flavor.rootDisk = server.flavor.disk
            i.status = server.status
            i.ip = server.public_v4
            i.createTime = server.created_at
            resp.instances.append(i)

        return resp

    @auth_required
    def CreateInstance(self, request: CreateInstanceRequest, context, claims: TokenClaims) -> CreateInstanceResponse:
        session = DBSession()
        user = get_user_from_claims(session, claims)

        client = create_client()

        # Creating the instance directly will not return the volume it also creates,
        # so the volume must be manually created
        volume = client.connection.create_volume(size=request.volume, image=request.imageName, bootable=True)

        os_instance = client.connection.create_server(request.name,
                                                      image=request.imageName,
                                                      flavor=request.flavorName,
                                                      boot_volume=volume.id,
                                                      )

        db_volume = models.Volume(id=volume.id, size=request.volume, owner_id=user.id, instance_id=os_instance.id)
        db_instance = models.Instance(id=os_instance.id, image_name=request.imageName, owner_id=user.id)
        session.add(db_instance)
        session.add(db_volume)
        session.commit()

        resp = CreateInstanceResponse()
        resp.instanceId = os_instance.id
        return resp

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

    def StartInstance(self, request, context):
        client = create_client()

    def DeleteInstance(self, request, context):
        client = create_client()
        client.connection.delete_server(name_or_id=request.instanceId)

        session = DBSession()
        instance = session.query(models.Instance).filter_by(id=request.instanceId).one()
        session.delete(instance)
        session.commit()

        return DeleteInstanceResponse()

    def StopInstance(self, request, context):
        client = create_client()
        client.connection.server

    def RebootInstance(self, request, context):
        client = create_client()
        client.connection.re


def add_to_server(server):
    g.add_InstanceServiceServicer_to_server(InstanceManagement(), server)

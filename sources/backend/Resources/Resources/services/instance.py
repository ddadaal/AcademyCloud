from datetime import datetime

from grpc import ServicerContext

import services.g.instance_pb2_grpc as g
from client import create_client
from client.client import Client
from db import DBSession, get_user_from_claims
import uuid

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
        session.close()

        # find the instances
        client = create_client()

        resp = GetInstancesResponse()
        for db_server in instances:
            server = client.connection.get_server_by_id(db_server.id)
            flavor = client.connection.get_flavor_by_id(server.flavor["id"])
            i = Instance()
            i.id = server.id
            i.name = server.name
            i.imageName = db_server.image_name
            i.flavor.name = flavor.name
            i.flavor.cpu = flavor.vcpus
            i.flavor.memory = flavor.ram
            i.flavor.rootDisk = flavor.disk
            i.status = server.status
            i.vmState = server.vm_state
            i.powerState = str(server.power_state)
            if server.task_state is not None:
                i.taskState = server.task_state
            i.ip = server.public_v4
            i.createTime = server.created_at
            resp.instances.append(i)

        return resp

    @auth_required
    def CreateInstance(self, request: CreateInstanceRequest, context, claims: TokenClaims) -> CreateInstanceResponse:
        session = DBSession()
        user = get_user_from_claims(session, claims)

        client = create_client()

        client.connection.list_flavors()

        # Creating the instance directly will not return the volume it also creates,
        # so the volume must be manually created
        volume = client.connection.create_volume(size=request.volume, image=request.imageName, bootable=True)

        os_instance = client.connection.create_server(request.name,
                                                      image=request.imageName,
                                                      flavor=request.flavorName,
                                                      boot_volume=volume.id,
                                                      )

        # manually get the flavor
        flavor = client.connection.get_flavor_by_id(os_instance.flavor["id"])

        resp = CreateInstanceResponse()
        resp.flavor.name = flavor.name
        resp.flavor.cpu = flavor.vcpus
        resp.flavor.memory = flavor.ram
        resp.flavor.rootDisk = flavor.disk

        resp.instanceId = os_instance.id

        db_volume = models.Volume(id=volume.id, size=request.volume, owner_id=user.id, instance_id=os_instance.id)
        db_instance = models.Instance(id=os_instance.id, image_name=request.imageName, owner_id=user.id)
        session.add(db_instance)
        session.add(db_volume)
        session.commit()

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
        compute = client.connection.compute
        compute.start_server(request.instanceId)
        return StartInstanceResponse()

    def DeleteInstance(self, request, context):
        client = create_client()
        # Get the server
        server = client.connection.get_server_by_id(id=request.instanceId)

        # Get the flavor
        flavor = client.connection.get_flavor_by_id(server.flavor["id"])

        resp = DeleteInstanceResponse()
        resp.flavor.name = flavor.name
        resp.flavor.cpu = flavor.vcpus
        resp.flavor.memory = flavor.ram
        resp.flavor.rootDisk = flavor.disk

        # Delete the server
        client.connection.delete_server(name_or_id=request.instanceId)

        # Get the server from db
        session = DBSession()
        instance = session.query(models.Instance).filter_by(id=request.instanceId).one()

        # Delete the volume
        for volume in instance.volumes:
            client.connection.delete_volume(name_or_id=volume.id)

        # Delete the instance from db
        session.delete(instance)
        session.commit()

        return resp

    def StopInstance(self, request, context):
        client = create_client()
        compute = client.connection.compute
        compute.stop_server(request.instanceId)
        return StopInstanceResponse()

    def RebootInstance(self, request, context):
        client = create_client()
        compute = client.connection.compute
        compute.reboot_server(request.instanceId, reboot_type="HARD" if request.hard else "SOFT")
        return RebootInstanceResponse()


def add_to_server(server):
    g.add_InstanceServiceServicer_to_server(InstanceManagement(), server)

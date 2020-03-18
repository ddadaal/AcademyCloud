from grpc import ServicerContext

import services.g.volume_pb2_grpc as g
from client import create_client
from db import DBSession, get_user_from_claims
from services.g.volume_pb2 import *
from utils.decorator import auth_required
from utils.token_claims import TokenClaims


class VolumeManagement(g.VolumeServiceServicer):
    @auth_required
    def GetVolumes(self, request, context, claims: TokenClaims):
        client = create_client()
        volumes = client.connection.list_volumes()

        session = DBSession()
        user = get_user_from_claims(session, claims)
        db_volumes = user.volumes
        session.close()

        resp = GetVolumesResponse()
        for volume in volumes:
            db_volume = next((i for i in db_volumes if str(i.id) == volume.id), None)
            if db_volume is None:
                continue
            v = Volume()
            v.size = volume.size
            v.id = volume.id
            v.createTime = volume.created_at
            attachment = volume.attachments[0]
            v.attachedToInstanceId = attachment.server_id
            v.attachedToInstanceName = client.connection.get_server_by_id(attachment.server_id).name
            v.attachedToDevice = attachment.device
            resp.volumes.append(v)

        return resp

def add_to_server(server):
    g.add_VolumeServiceServicer_to_server(VolumeManagement(), server)

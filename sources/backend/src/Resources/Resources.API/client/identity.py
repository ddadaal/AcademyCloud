from openstack.identity.identity_service import IdentityService as OSIdentityService


class IdentityService:
    os_identity: OSIdentityService

    def __init__(self, identity: OSIdentityService):
        self.identity = identity

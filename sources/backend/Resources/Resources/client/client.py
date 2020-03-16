from client.extended_apis import ExtendedApis
from client.identity import IdentityService
from client.models import Project, Domain, Token, RoleName
from openstack.connection import Connection
import config


class Client:
    extended_apis: ExtendedApis
    connection: Connection

    identity: IdentityService

    token: Token

    def __init__(self, os_connection: Connection):
        self.connection = os_connection
        self.extended_apis = ExtendedApis(os_connection)
        self.token = self.extended_apis.get_token_information()
        self.identity = IdentityService(os_connection.identity)


def create_client():
    return Client(Connection(
        auth_url=config.openstack_auth_url,
        domain_name=config.openstack_domain_name,
        username=config.openstack_admin_username,
        password=config.openstack_admin_password,
        project_name=config.openstack_project_name,
    ))

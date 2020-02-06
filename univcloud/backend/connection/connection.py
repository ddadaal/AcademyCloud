from urllib.parse import urljoin
from typing import Optional, List, Union, Tuple, TypedDict, Literal, Protocol, cast

from munch import Munch

from connection.extended_apis import get_scopeable_project_ids, get_scopeable_domain_id, get_roles_user_has_on_domain, \
    get_token_information
from connection.identity import IdentityService
from connection.models import Project, Domain
from openstack.connection import Connection as OpenStackConnection
import requests
import config


class NoScopeableDomainOrProjectException(Exception):
    """This account has no domain or project assigned."""
    pass


class Scope(Protocol):
    type: Union[Literal["project"], Literal["domain"]]
    id: str
    is_admin: bool


class Connection:
    os_connection: OpenStackConnection
    identity: IdentityService
    current_scope: Scope

    domain_id: str

    def __init__(self, os_connection: OpenStackConnection, domain_id: str):
        self.os_connection = os_connection

        token = get_token_information(os_connection.auth["auth_url"], os_connection.auth_token)

        if os_connection.current_project_id is not None:
            self.current_scope = cast(Scope, Munch(type="project", id=os_connection.current_project_id,
                                                   is_admin=any(role.name == "admin" for role in token.roles)))
        else:
            self.current_scope = cast(Scope, Munch(type="domain", id=domain_id,
                                                   is_admin=any(role.name == "admin" for role in token.roles
                                                                )))

            self.identity = IdentityService(os_connection.identity)

    def connect_as_project(self, project_id: str) -> "Connection":
        return Connection(self.os_connection.connect_as(project_id=project_id), self.domain_id)

    @property
    def auth_token(self) -> str:
        return self.os_connection.auth_token

    @property
    def current_project_id(self) -> str:
        return self.os_connection.current_project_id

    @staticmethod
    def connect(username: str, password: str, domain_name: str, project_name: Optional[str] = None) -> "Connection":
        # Try connect with credentials and login as unscoped
        conn = OpenStackConnection(auth_url=config.openstack_auth_url,
                                   user_domain_name=domain_name,
                                   username=username,
                                   password=password,
                                   )

        # Try scope to projects

        projects = get_scopeable_project_ids(config.openstack_auth_url, conn.auth_token)
        if len(projects) == 0:
            # The account doesn't have a scopeable project
            # Try domain
            domains = get_scopeable_domain_id(config.openstack_auth_url, conn.auth_token)
            if len(domains) == 0:
                raise NoScopeableDomainOrProjectException()

            # It's a domain. Reconnect as domain scoped
            domain = domains[0]
            conn = conn.connect_as(domain_id=domain.id)
            return Connection(conn, domain.id)
        else:
            # Scope to project
            project = projects[0]
            conn = conn.connect_as(project_id=project.id)
            return Connection(conn, project.domain_id)


if __name__ == '__main__':
    Connection.connect("admin", "admin", "NJU")
    # get("admin", "admin", "NJU")

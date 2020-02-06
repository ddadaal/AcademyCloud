from urllib.parse import urljoin
from typing import Optional, List, Union, Tuple

from connection.identity import IdentityService
from connection.models import Project, Domain
from openstack.connection import Connection as OpenStackConnection
import requests
import config


def request_with_auth_token(url: str, auth_token: str) -> requests.Response:
    return requests.get(url, headers={"X-Auth-Token": auth_token, "Content-Type": "application/json"})


def get_scopeable_project_ids(auth_url: str, auth_token: str) -> List[Project]:
    return request_with_auth_token(urljoin(auth_url, "auth/projects"), auth_token).json().projects


def get_scopeable_domain_id(auth_url: str, auth_token: str) -> List[Domain]:
    return request_with_auth_token(urljoin(auth_url, "auth/domains"), auth_token).json().domains


class NoScopeableDomainOrProjectException(Exception):
    """This account has no domain or project assigned."""
    pass


class Connection:
    os_connection: OpenStackConnection
    identity: IdentityService
    current_scope: Union[Tuple["project", str], Tuple["domain", str]]

    def __init__(self, os_connection: OpenStackConnection):
        self.os_connection = os_connection
        self.identity = IdentityService(os_connection.identity)

    def connect_as_project(self, project_id: str) -> "Connection":
        return Connection(self.os_connection.connect_as_project(project_id))

    @property
    def auth_url(self) -> str:
        return self.os_connection.auth_token

    @property
    def current_project(self) -> str:
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

        projects: List[Project] = get_scopeable_project_ids(config.openstack_auth_url, conn.auth_token)

        if len(projects) == 0:
            # The account doesn't have a scopeable project
            # Try domain
            domains = get_scopeable_domain_id(config.openstack_auth_url, conn.auth_token)
            if len(domains) == 0:
                raise NoScopeableDomainOrProjectException()

            # It's a domain. Reconnect as scopeable domain
            conn.connect_as(domain_name=domains[0].id)
        else:
            # Scope to projects
            conn.connect_as_project(projects[0].id)

        return Connection(conn)


if __name__ == '__main__':
    Connection.connect("admin", "admin", "NJU")
    # get("admin", "admin", "NJU")

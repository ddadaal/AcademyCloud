import functools
from dataclasses import dataclass
from urllib.parse import urljoin
from typing import Optional, List, Union, Tuple, TypedDict, Literal, Protocol, cast

from munch import Munch

from connection.extended_apis import ExtendedApis
from connection.identity import IdentityService
from connection.models import Project, Domain, Token, RoleName
from openstack.connection import Connection as OpenStackConnection
import config
import cachetools


@dataclass
class Scope:
    type: Union[Literal["project"], Literal["domain"]]
    id: str
    name: str
    role: RoleName


@dataclass
class ScopeableTarget:
    type: Union[Literal["project"], Literal["domain"]]
    id: str
    name: str


class Connection:
    extended_apis: ExtendedApis
    os_connection: OpenStackConnection

    identity: IdentityService

    token: Token
    domain_id: str

    def __init__(self, os_connection: OpenStackConnection, domain_id: str):
        self.os_connection = os_connection
        self.extended_apis = ExtendedApis(os_connection)
        self.domain_id = domain_id
        self.token = self.extended_apis.get_token_information()
        self.identity = IdentityService(os_connection.identity)

    def connect_as_project(self, project_id: str) -> "Connection":
        return Connection(self.os_connection.connect_as(project_id=project_id), self.domain_id)

    @property
    def current_scope(self) -> Scope:

        highest_role = cast(RoleName, self.token.roles[0].name)

        if "project" in self.token:
            return Scope(type="project",
                         id=self.token.project.id, name=self.token.project.name,
                         role=highest_role)
        else:
            return Scope(type="domain",
                         id=self.token.domain.id, name=self.token.domain.name,
                         role=highest_role)

    @property
    def auth_token(self) -> str:
        return self.os_connection.auth_token

    @property
    def current_project_id(self) -> str:
        return self.os_connection.current_project_id


@dataclass(frozen=True, eq=True)
class ScopedAuth:
    username: str
    password: str
    domain_name: str
    project_name: Optional[str]


@functools.lru_cache(120)
def scoped_connect(auth: ScopedAuth) -> "Connection":
    if auth.project_name:
        # project scoped auth
        return Connection(OpenStackConnection(
            auth_url=config.openstack_auth_url,
            user_domain_name=auth.domain_name,
            username=auth.username,
            password=auth.password,
            project_domain_name=auth.domain_name,
            project_name=auth.project_name,
        ), auth.domain_name)
    else:
        # domain scoped auth
        return Connection(OpenStackConnection(
            auth_url=config.openstack_auth_url,
            user_domain_name=auth.domain_name,
            username=auth.username,
            password=auth.password,
            domain_name=auth.domain_name,
        ), auth.domain_name)


def get_scopeable_targets(username: str, password: str, domain_name: str) -> List[ScopeableTarget]:
    # Try connect with credentials and login as unscoped
    conn = OpenStackConnection(auth_url=config.openstack_auth_url,
                               user_domain_name=domain_name,
                               username=username,
                               password=password,
                               )

    # Get more apis
    extended_apis = ExtendedApis(conn)

    # Try scope to projects
    projects = extended_apis.get_scopeable_projects()
    if len(projects) == 0:
        # The account doesn't have a scopeable project
        # Try domain
        domains = extended_apis.get_scopeable_domains()
        if len(domains) == 0:
            return []

        # A domain is scopeable. Return domain.
        domain = domains[0]
        return [ScopeableTarget(type="domain", id=domain.id, name=domain.name)]
    else:
        # Return all scopeable project
        return [ScopeableTarget(type="project", id=project.id, name=project.name)
                for project in projects]

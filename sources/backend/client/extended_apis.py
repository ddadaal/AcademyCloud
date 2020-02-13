from typing import List, Dict, Optional
from urllib.parse import urljoin

import requests
from munch import Munch

from openstack.connection import Connection as OpenStackConnection

from client.models import Project, Domain, Token, Entity


class ExtendedApis:
    auth_url: str
    auth_token: str

    def __init__(self, os_connection: OpenStackConnection):
        self.auth_token = os_connection.auth_token
        self.auth_url = os_connection.auth["auth_url"]

    def request_with_auth_token(self, url: str, extra_headers: Optional[Dict[str, str]] = None) -> requests.Response:
        if not extra_headers:
            extra_headers = {}
        return requests.get(urljoin(self.auth_url, url),
                            headers={"X-Auth-Token": self.auth_token,
                                     "Content-Type": "application/json",
                                     **extra_headers})

    def get_scopeable_projects(self) -> List[Project]:
        resp = self.request_with_auth_token("auth/projects").json()
        return Munch.fromDict(resp["projects"])

    def get_scopeable_domains(self) -> List[Domain]:
        resp = self.request_with_auth_token("auth/domains").json()
        return Munch.fromDict(resp["domains"])

    def get_roles_user_has_on_domain(self, user_id: str, domain_id: str) -> List[Entity]:
        resp = self.request_with_auth_token(
            f"domains/{domain_id}/users/{user_id}/roles"
        ).json()
        return Munch.fromDict(resp["roles"])

    def get_roles_user_has_on_project(self, user_id: str, project_id: str) -> List[Entity]:
        resp = self.request_with_auth_token(
            f"projects/{project_id}/users/{user_id}/roles"
        ).json()
        return Munch.fromDict(resp["roles"])

    def get_token_information(self) -> Token:
        resp = self.request_with_auth_token("auth/tokens", {"X-Subject-Token": self.auth_token}).json()

        return Munch.fromDict(resp["token"])

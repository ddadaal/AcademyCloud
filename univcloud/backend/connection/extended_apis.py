from typing import List
from urllib.parse import urljoin

import requests
from munch import Munch

from connection.models import Project, Domain, Role, Token


def request_with_auth_token(url: str, auth_token: str) -> requests.Response:
    return requests.get(url, headers={"X-Auth-Token": auth_token, "Content-Type": "application/json"})


def get_scopeable_project_ids(auth_url: str, auth_token: str) -> List[Project]:
    resp = request_with_auth_token(urljoin(auth_url, "auth/projects"), auth_token).json()
    return Munch.fromDict(resp["projects"])


def get_scopeable_domain_id(auth_url: str, auth_token: str) -> List[Domain]:
    resp = request_with_auth_token(urljoin(auth_url, "auth/domains"), auth_token).json()
    return Munch.fromDict(resp["domains"])


def get_roles_user_has_on_domain(auth_url: str, auth_token: str, user_id: str, domain_id: str) -> List[Role]:
    resp = request_with_auth_token(
        urljoin(auth_url,
                "/v3/domains/{domain_id}/users/{user_id}/roles".format(domain_id=domain_id, user_id=user_id)),
        auth_token).json()
    return Munch.fromDict(resp["roles"])


def get_roles_user_has_on_project(auth_url: str, auth_token: str, user_id: str, project_id: str) -> List[Role]:
    resp = request_with_auth_token(
        urljoin(auth_url,
                "/v3/projects/{project_id}/users/{user_id}/roles".format(project_id=project_id,
                                                                         user_id=user_id)), auth_token).json()
    return Munch.fromDict(resp["roles"])


def get_token_information(auth_url: str, auth_token: str) -> Token:
    resp = requests.get("{auth_url}auth/tokens".format(auth_url=auth_url),
                        headers={"X-Auth-Token": auth_token, "X-Subject-Token": auth_token,
                                 "Content-Type": "application/json"}).json()

    return Munch.fromDict(resp["token"])

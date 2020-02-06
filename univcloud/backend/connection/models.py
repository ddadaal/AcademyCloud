from typing import Protocol, Optional, List


class User(Protocol):
    id: str
    name: str


class Project(Protocol):
    enabled: bool
    id: str
    domain_id: str
    name: str
    description: str


class Domain(Protocol):
    enabled: bool
    id: str
    name: str
    description: str


class Role(Protocol):
    id: str
    name: str


class RoleComplete(Role):
    description: Optional[str]
    domain_id: Optional[str]


class Token(Protocol):
    is_domain: bool
    method: List[str]
    roles: List[Role]
    user: User

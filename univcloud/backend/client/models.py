from typing import Protocol, Optional, List, Union, Literal


class Entity(Protocol):
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


RoleName = Union[Literal["admin"], Literal["member"]]


class Role(Entity):
    name: RoleName
    description: Optional[str]
    domain_id: Optional[str]


class Token(Protocol):
    is_domain: bool

    # Either
    domain: Entity
    # Or
    project: Entity

    methods: List[str]
    roles: List[Entity]
    user: Entity

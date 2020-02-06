from typing import Protocol


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

from enum import Enum
from typing import TypedDict, Optional


class UserRole(str, Enum):
    Admin = "Admin",
    Member = "Member"


class TokenClaims(TypedDict):
    System: bool
    Social: bool
    UserId: str
    DomainId: str
    ProjectId: Optional[str]
    Role: UserRole

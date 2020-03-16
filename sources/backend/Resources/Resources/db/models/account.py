from enum import Enum

from db import db
from db.models.utils import GUID


class User(db.Model):
    """用户。
    """

    id = db.Column(GUID(), primary_key=True)

    name = db.Column(db.String)
    email = db.Column(db.String)

    is_system_admin = db.Column(db.Boolean, index=True, default=False)


class Domain(db.Model):
    """域。
    """
    id = db.Column(GUID(), primary_key=True)
    name = db.Column(db.String, unique=True, nullable=True)


class Project(db.Model):
    """项目。
    """
    id = db.Column(GUID(), primary_key=True)
    name = db.Column(db.String)

    domain_id = db.Column(GUID(), index=True)


class UserDomainAssignment(db.Model):
    """用户对域的分配关系。
    """
    id = db.Column(GUID(), primary_key=True)
    user_id = db.Column(GUID(), index=True)
    domain_id = db.Column(GUID(), index=True)
    role = db.Column(db.Enum(Role))

    school_id = db.Column(db.String)


class UserProjectAssignment(db.Model):
    """用户对项目的分配关系。

    """
    id = db.Column(GUID(), primary_key=True)
    user_id = db.Column(GUID(), index=True)
    project_id = db.Column(GUID(), index=True)
    role = db.Column(db.Enum(Role))

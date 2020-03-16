from enum import Enum

from db import db
from db.models.utils import GUID

class UserResourceUsage(db.Model):
    """
    用户资源使用量记录。
    若用户改变对资源的使用量，则需要在这里加一条
    """
    id = db.Column(GUID(), primary_key=True)

    domain_id = db.Column(db.String, index=True)
    project_id = db.Column(db.String, index=True)
    user_id = db.Column(db.String, index=True)


class Instance(db.Model):
    """
    实例
    """
    id = db.Column(GUID(), primary_key=True)

    """对应的OpenStack系统中的ID"""
    os_id = db.Column(GUID())

    cpu = db.Column(db.Integer)
    """单位: MB"""
    memory = db.Column(db.Integer)


class Volume(db.Model):
    """云硬盘"""

    id = db.Column(GUID(), primary_key=True)

    os_id = db.Column(GUID())

    size = db.Column(db.Integer)


class VolumeMount(db.Model):
    """云硬盘挂载情况"""
    id = db.Column(GUID(), primary_key=True)

    instance_id = db.Column(GUID())
    volume_id = db.Column(GUID())
    mount_time = db.Column(db.DateTime)

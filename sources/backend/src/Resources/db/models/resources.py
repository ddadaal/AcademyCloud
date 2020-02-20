from enum import Enum

from db import db
from db.models.utils import GUID


class AssignerType(str, Enum):
    System = "System",
    Domain = "Domain",
    Project = "Project",


class ResourceAssignment(db.Model):
    """
    资源分配记录，指系统对域、域对项目和项目对用户的资源分配额
    assigner_type指定分配者的类型（系统、域、项目）
    end_time为null表示为目前启用的分配量
    本表不可修改，若资源分配额更改，则增加一条，并把原来的目前分配量的end_time设置为结束时间
    """

    id = db.Column(GUID(), primary_key=True)

    assigner_type = db.Column(db.Enum(AssignerType), index=True)

    assigner_id = db.Column(db.String, index=True)
    assignee_id = db.Column(db.String, index=True)

    start_time = db.Column(db.DateTime, index=True)
    end_time = db.Column(db.DateTime, index=True, nullable=True)

    vcpu = db.Column(db.Integer)
    memory_gb = db.Column(db.Integer)
    disk_gb = db.Column(db.Integer)
    floating_ip = db.Column(db.Integer)


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

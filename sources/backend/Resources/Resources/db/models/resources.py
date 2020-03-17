from enum import Enum

from sqlalchemy import Column, String, Integer, DateTime, ForeignKey
from sqlalchemy.orm import relationship

from db.models.utils import GUID, Base


class Instance(Base):
    """
    实例
    """
    __tablename__ = "instance"
    id = Column(GUID(), primary_key=True)

    """对应的OpenStack系统中的ID"""
    os_id = Column(GUID())

    cpu = Column(Integer)
    """单位: MB"""
    memory = Column(Integer)

    owner_id = Column(GUID(), ForeignKey("user.id"))

    volumes = relationship("Volume")


class Volume(Base):
    """云硬盘"""
    __tablename__ = "volume"

    id = Column(GUID(), primary_key=True)

    os_id = Column(GUID())

    """" 单位GB"""
    size = Column(Integer)
    owner_id = Column(GUID(), ForeignKey("user.id"))

    instance_id = Column(GUID(), ForeignKey("instance.id"))

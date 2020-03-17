from enum import Enum

from sqlalchemy import Column, String, Integer, DateTime, ForeignKey
from sqlalchemy.orm import relationship

from db.models.utils import GUID, Base


class Instance(Base):
    """
    实例
    """
    __tablename__ = "instance"

    """直接对应openstack系统里的ID"""
    id = Column(GUID(), primary_key=True)

    owner_id = Column(GUID(), ForeignKey("user.id"), nullable=False)

    volumes = relationship("Volume")


class Volume(Base):
    """云硬盘"""
    __tablename__ = "volume"

    id = Column(GUID(), primary_key=True)

    os_id = Column(GUID())

    """" 单位GB"""
    size = Column(Integer)
    owner_id = Column(GUID(), ForeignKey("user.id"), nullable=False)

    instance_id = Column(GUID(), ForeignKey("instance.id"), nullable=True)

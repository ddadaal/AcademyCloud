from enum import Enum

from sqlalchemy import Column, String, Boolean
from sqlalchemy.orm import relationship

from db.models.utils import GUID, Base


class User(Base):
    """用户。
    """

    __tablename__ = "user"

    id = Column(GUID(), primary_key=True)

    instances = relationship("Instance")




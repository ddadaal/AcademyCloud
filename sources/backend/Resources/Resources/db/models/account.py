from enum import Enum

from sqlalchemy import Column, String, Boolean
from sqlalchemy.orm import relationship

from db.models.utils import GUID, Base


class User(Base):
    """用户。
    代表的是整个系统中的一个UserProjectAssignment
    """

    __tablename__ = "user"

    # The id of UserProjectAssignment
    id = Column(GUID(), primary_key=True)

    instances = relationship("Instance")




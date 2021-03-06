from enum import Enum

from sqlalchemy import Column, String, Boolean
from sqlalchemy.orm import relationship

from db.models.utils import GUID, Base


class ProjectUser(Base):
    """用户。
    代表的是整个系统中的一个UserProjectAssignment
    """

    __tablename__ = "project_user"

    # The id of UserProjectAssignment
    id = Column(GUID, primary_key=True)

    user_id = Column(GUID, nullable=False)
    project_id = Column(GUID, nullable=False)

    instances = relationship("Instance", cascade="save-update, merge, delete")
    volumes = relationship("Volume", cascade="save-update, merge, delete")




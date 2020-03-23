from typing import Optional

from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
import config

# 初始化数据库连接:
from db.models.account import ProjectUser
from db.models.utils import Base
import uuid

from utils.token_claims import TokenClaims

# Create database if not exists
engine = create_engine(config.database_url, echo=True)
engine.execute(f"CREATE DATABASE IF NOT EXISTS {config.database_name};")
engine.execute(f"USE {config.database_name};")
engine.dispose()

# 创建DBSession类型:
engine = create_engine(f"{config.database_url}/{config.database_name}", echo=True)
DBSession = sessionmaker(bind=engine)


# Will Throw if no user is or multiple users are found.
def get_user_from_claims(session, claims: TokenClaims) -> ProjectUser:
    return get_user(session, claims["UserId"], claims["ProjectId"])


def get_user(session, user_id: str, project_id: str) -> ProjectUser:
    return session.query(ProjectUser).filter_by(user_id=user_id, project_id=project_id).one()

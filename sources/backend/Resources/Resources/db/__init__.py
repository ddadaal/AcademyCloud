from typing import Optional

from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
import config

# 初始化数据库连接:
from db.models.account import User
from db.models.utils import Base
import uuid

from utils.token_claims import TokenClaims

engine = create_engine(config.database_url, echo=True)

# 创建DBSession类型:
DBSession = sessionmaker(bind=engine)


# Will Throw if no user is or multiple users are found.
def get_user_from_claims(claims: TokenClaims) -> User:
    return get_user(claims["UserId"], claims["ProjectId"])


def get_user(user_id: str, project_id: str) -> User:
    session = DBSession()
    return session.query(User).filter_by(user_id=user_id, project_id=project_id).one()

from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
import config


# 初始化数据库连接:
from db.models.account import User
from db.models.utils import Base
import uuid

engine = create_engine(config.database_url, echo=True)

# 创建DBSession类型:
DBSession = sessionmaker(bind=engine)


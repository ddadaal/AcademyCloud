from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
import config


# 初始化数据库连接:
engine = create_engine(config.database_url)

# 创建DBSession类型:
DBSession = sessionmaker(bind=engine)

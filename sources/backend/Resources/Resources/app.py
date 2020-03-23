import uuid

import grpc
import services
from concurrent.futures import ThreadPoolExecutor

from db import engine, DBSession, User, Base
from db.models import Instance, Volume

port = 80

# init db

# 初始化数据
Base.metadata.create_all(engine)

# 加一条初始数据方便测试
# Token：eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJTeXN0ZW0iOiJGYWxzZSIsIlNvY2lhbCI6IlRydWUiLCJVc2VySWQiOiI5ZGM2OWI5My1hMjYxLTRjZjUtOTgzNi1kYjdhMjIxNWU5YzEiLCJEb21haW5JZCI6ImE3NDc2NzU2LTg1OGMtNDRjNS1iYzc3LTRjNDEyMTJiMzY0ZCIsIlByb2plY3RJZCI6Ijk5ZDc2YTZjLTA1ZGEtNDIyYy04NzQ2LTczMjhmN2NkMjIxMiIsIlJvbGUiOiJBZG1pbiIsIm5iZiI6MTU4NDQyODQ1MCwiZXhwIjoxNTg0NDMyMDUwLCJpYXQiOjE1ODQ0Mjg0NTAsImlzcyI6Imh0dHBzLy9hY2FkZW15Y2xvdWQuY29tIiwiYXVkIjoiaHR0cHMvL2FjYWRlbXljbG91ZC5jb20ifQ.9Acz-Y4ULbsScE_AidnyObv-FB7kceCLPWwhPaWYGRg

session = DBSession()
user_id = "12848b44-682f-11ea-a279-d46d6d2a6673"
test_user = User(id=user_id, user_id="9dc69b93-a261-4cf5-9836-db7a2215e9c1",
                 project_id="99d76a6c-05da-422c-8746-7328f7cd2212")
session.merge(test_user)
instance_id = "6bc2a6f8-6cbb-4996-9d20-58c68a27776a"
instance = Instance(id=instance_id, owner_id=user_id, image_name="cirros")
session.merge(instance)
volume_id = "b937cd1b-1dea-4440-b072-83d0d011d202"
volume = Volume(id=volume_id, owner_id=user_id, instance_id=instance_id)
session.merge(volume)
session.commit()

server = grpc.server(ThreadPoolExecutor(max_workers=10))
services.init_services(server)

print(f'Server running at localhost:{port}')

server.add_insecure_port(f'[::]:{port}')
server.start()
server.wait_for_termination()

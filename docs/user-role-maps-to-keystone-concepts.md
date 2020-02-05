# Keystone权限

| 业务角色 | Keystone用户 | domain | project | role   | 权限                                                   |
| -------- | ------------ | ------ | ------- | ------ | ------------------------------------------------------ |
| 平台管理 | admin        |        |         | admin  | 所有权限                                               |
| 学校管理 | admin        | NJU    |         | admin  | 管理一个domain下的所有project, user等，查看所有domain  |
| 老师     | 67           | NJU    | 67      | admin  | 管理project下的用户和资源等，查看domain里的所有project |
| 同学     | cjd          | NJU    | 67      | member | 管理自己名下的资源                                     |

## 相关命令


```bash
# 使用平台管理权限
. /root/admin-openrc

# 创建NJU domain 
openstack domain create NJU

# 在NJU中创建学校管理admin用户，密码为admin
openstack user create admin --password admin --domain NJU

# 给NJU.admin用户给予admin角色
openstack role add --domain NJU \
    --user admin --user-domain NJU admin

# 创建NJU.67 project
openstack project create 67 --domain NJU

# 创建67老师
openstack user create 67 --password 67 --domain NJU

# 给NJU.67老师分配到NJU.67 project中给予admin角色 
openstack role add --project 67 --project-domain NJU \
    --user 67 --user-domain NJU admin

# 创建NJU.cjd用户
openstack user create cjd --password cjd --domain NJU

# 给NJU.cjd用户分配到NJU.67 project中给予member角色
openstack role add --project 67 --project-domain NJU \
    --user cjd --user-domain NJU member

```
# Keystone权限

| 业务角色 | Keystone用户 | domain  | project | role   | 权限                                                   |
| -------- | ------------ | ------- | ------- | ------ | ------------------------------------------------------ |
| 平台管理 | admin        | Default | default | admin  | 所有权限                                               |
| 学校管理 | admin        | NJU     |         | admin  | 管理一个domain下的所有project, user等，查看所有domain  |
| 老师     | 67           | NJU     | 67      | admin  | 管理project下的用户和资源等，查看domain里的所有project |
| 同学     | cjd          | NJU     | 67      | member | 管理自己名下的资源                                     |

## 相关命令

```bash
# http://www.florentflament.com/blog/setting-keystone-v3-domains.html

# 1. Train release还没有默认使用multi-domain，所以得使用policy.json手动开启multi-domain
# 使用系统admin
. /root/admin-openrc

# 创建admin_domain domain
# 记住这个domain的ID
openstack domain create admin_domain

# 创建cloud_admin用户到admin_domain domain
openstack user create cloud_admin --password cloud_admin --domain admin_domain

# 给cloud_admin用户admin权限到admin_domain
openstack role add --user cloud_admin --domain admin_domain admin

# 复制正确的policy.json
cp /vagrant/scripts/services/keystone/policy.json /etc/keystone/

# ！！修改policy.json中的admin_domain为admin_domain的ID
export ID_ADMIN_DOMAIN=eb705326809647e4a5f5fe1ba5045f28
sed s/admin_domain_id/${ID_ADMIN_DOMAIN}/ -i /etc/keystone/policy.json

# 2. 从现在开始，admin只能管理Default domain里的资源
#    以后的操作要使用cloud_admin才能进行

# 创建cloud-admin-openrc
# 复制并应用cloud-admin-openrc

# 创建NJU domain
openstack domain create NJU

# 在NJU domain中创建admin用户
# 记住admin用户的id
openstack user create admin --password admin --domain NJU

# 给admin admin角色在NJU中
# 直接domain和user限制是无法获得这个用户的，必须用ID
# openstack role add --user-domain NJU --user admin --domain NJU admin
openstack role add --user 14a36749d4bd42d78e7c05b67a5c954e --domain NJU admin

# 要操作NJU domain，必须用NJU的admin
# 复制并应用nju-admin-openrc

# 在NJU domain中创建67用户
# 记住ID
openstack user create 67 --password 67 --domain NJU

# 在NJU Admin中创建67 project
openstack project create 67 --domain NJU


```


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
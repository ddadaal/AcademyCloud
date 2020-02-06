# 权限问题解决方案

| 业务角色 | univcloud用户 | univcloud role | Keystone用户 | domain  | project | role                |
| -------- | ------------- | -------------- | ------------ | ------- | ------- | ------------------- |
| 平台管理 | admin         | cloud-admin    | admin        | Default | default | admin (system)      |
| 学校管理 | nju-admin     | school-admin   | admin        | NJU     | default | admin (NJU domain)  |
| 老师     | 67            | teacher        | 67           | NJU     | 67      | admin (67 project)  |
| 同学     | cjd           | student        | cjd          | NJU     | 67      | member (67 project) |


## 3级admin详细解释

1. Train release已经默认使用了default roles
2. 一个domain下的admin账号
   1. 只对domain有admin，以domain scope登录（登录时指定domain，不指定project）：
      1. 可以显示所有的domain（隐藏）
      2. 只能显示本domain下的project和user（可以接受）
      3. 能够创建domain（UI隐藏）
      4. 能够创建project（接受只能在当前domain创建）
      5. 不能显示instances
   2. 只某一个project有admin，以project scoped登录（在登录时指定project）：
      1. 可以显示所有domain（隐藏）
      2. 能够显示所有domain下的project和user（project过滤只显示当前domain的；user过滤，只显示当前的project的）
      3. 能够创建domain（UI隐藏）
      4. 能够创建project（UI隐藏）
      5. 可以显示当前project下的所有instances（可以接受）
3. 一个domain下，某个project的member账号
   1. 不能显示任何project和domain（可以接受）
   2. 不能显示自己为member的project的user（可以接受）
   3. 不能创建domain和project（可以接受）
   4. 可以显示自己的instances (list_servers())（可以接受）
4. 一个domain下不为任何project的账号：
   1. 无法进行任何操作，报The service catalog is empty（可以接受，显示让它找domain admin分配project）

## 实现细节

业务假设：

1. domain admin不能是任何一个domain的project的成员
2. 每个用户必须属于至少一个project，只接受Member和admin用户；admin表示老师，member表示学生
3. 不适用group功能

登录过程：

1. 登录后，再使用root账号登录一次获得所有role assignment，然后找出当前登录用户的所有属于的projects
2. 若当前用户是一个domain的admin，则显示domain admin界面，登陆结束。若否：
3. 找出primary_project或者任意一个project，并以此Project重新登录当前用户
4. 若没有指定任何project，应该报错，拒绝登录
5. 用户可以在界面里切换使用的project




## ~~尝试配置三级admin相关命令~~

Deprecated：train release已经使用了三级Admin

```bash
# http://www.florentflament.com/blog/setting-keystone-v3-domains.html

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
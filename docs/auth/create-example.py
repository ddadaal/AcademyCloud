import openstack

# 以cloud_admin为名创建domain scoped的链接

conn = openstack.connect(
    auth_url="http://controller:5000/v3",
    username="admin",
    password="ADMIN_PASS",
    user_domain_name="Default",
    project_name="admin",
    project_domain_name="Default"
)

# 创建NJU domain
nju_domain = conn.create_domain("NJU", description="NJU")
# 获取NJU domain的ID
print(nju_domain)

# 在NJU domain中创建admin用户
nju_admin = conn.create_user(
    "admin", password="admin", domain_id=nju_domain.id)
# 获取admin用户的ID
print(nju_admin)

# 给admin用户NJU domain的admin权限
conn.grant_role("admin", user=nju_admin.id, domain=nju_domain.id)

# 以nju_admin重新连接
conn = openstack.connect(auth_url="http://controller:5000/v3", username="admin", password="admin",
                         user_domain_id=nju_domain.id)

# 在NJU里创建67用户
user_67 = conn.create_user("67", password="67", domain_id=nju_domain.id)

# 在NJU里创建67 project
project_67 = conn.create_project("67", domain_id=nju_admin.id)

# 给67用户67 project的admin权限
conn.grant_role("admin", user=user_67.id,
                project=project_67.id, domain=nju_domain.id)

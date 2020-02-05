# 鉴权方法

```python

# Domain scoped
conn = openstack.connect(
    auth_url="http://controller:5000/v3",
    username="cloud_admin",
    password="cloud_admin",
    user_domain_name="admin_domain",
    domain_name="admin_domain"
)



```
from typing import Optional

import openstack

openstack.enable_logging(debug=True)


def create_connection(domain_name: str, project_name: Optional[str], username: str,
                      password: str) -> openstack.connection.Connection:
    return openstack.connect(
        auth_url="http://controller:5000/v3",
        project_domain_name=domain_name,
        project_name=project_name,
        user_domain_name=domain_name,
        username=username,
        password=password,
        region_name="RegionOne",
        app_name="univcoud",
        app_version="1.0",
    )


if __name__ == '__main__':

    conn = openstack.connect(
        auth_url="http://controller:5000/v3",
        username="cloud_admin",
        password="cloud_admin",
        user_domain_name="admin_domain",
        domain_name="admin_domain"
    )

    for user in conn.list_domains():
        print(user)

    # connection = create_connection("admin_domain", None, "cloud_admin", "cloud_admin")
    # # domain= connection.identity.find_domain("NJU")
    # for user in connection.list_users():
    #     print(user)

# 安装和部署OpenStack

- `vagrant`和`virtualbox`来管理和部署虚拟机和网络
- 使用`virtualbox internal network`实现多机之间的连接
  - 主机：192.168.56.1，默认:w
  - Controller Node: 192.168.56.2
  - Compute Node: 192.168.56.3
  - 使用`vagrant-proxyconf`为各个虚拟机设置代理到主机的SS服务器`http://10.0.2.2:1080`
- `generic/centos7`镜像
    - `ubuntu 18.04`的仓库中不存在`placement-api`，自己使用pypi安装+apache2部署，会在访问终结点的时候出现403错误，无法解决

# Vagrant的配置

vagrant-hostmanager容易卡住，所以手动维护hosts文件

<!-- 需要安装两个插件
```
vagrant plugin install vagrant-hostmanager
vagrant plugin install vagrant-vbguest
``` -->

# Controller Node

需要安装的包：
- [基础包](#openstack)
- [数据库](#database)
- [消息队列](#rabbitmq)
- [memcached](#memcached)
- [etcd](#etcd)
- [配置网络](#network-of-controller)

需要安装以下服务：
- Identity ([keystone](#keystone))
- Image ([glance](#glance))
- Placement ([placement](#placement))
- Compute (management portion)
- Networking (management portion, various network agents)
- Dashboard (horizon)

```bash
/vagrant/scripts/init-controller.sh
```

# Compute Node

包需要安装和配置：
- [基础包](#openstack)

需要安装服务：
- Compute (hypervisor portion)
- a Network service agent

# 安装脚本
## network of controller

https://docs.openstack.org/install-guide/environment-networking-controller.html

```bash
/vagrant/scripts/packages/network/controller.sh
```

## openstack

https://docs.openstack.org/install-guide/environment-packages-rdo.html

```bash
/vagrant/scripts/packages/openstack/install.sh
```

## database

https://docs.openstack.org/install-guide/environment-sql-database-rdo.html

```bash
/vagrant/scripts/packages/database/install.sh
```

## rabbitmq

| 信息         | 值                     | 要修改的地方 |
| ------------ | ---------------------- | ------------ |
| 用户名和密码 | openstack, RABIIT_PASS | install.sh   |

https://docs.openstack.org/install-guide/environment-messaging-rdo.html

```bash
/vagrant/scripts/packages/rabbitmq/install.sh
```

## memcached

https://docs.openstack.org/install-guide/environment-memcached-rdo.html

```bash
/vagrant/scripts/packages/memcached/install.sh
```

## etcd

https://docs.openstack.org/install-guide/environment-etcd-rdo.html

```bash
/vagrant/scripts/packages/etcd/install.sh
```

## keystone

| 信息               | 值                            | 要修改的地方                            |
| ------------------ | ----------------------------- | --------------------------------------- |
| 数据库用户名和密码 | `keystone`, `KEYSTONE_DBPASS` | keystone.conf的connection =，install.sh |

```bash
# Install
# https://docs.openstack.org/keystone/train/install/keystone-install-rdo.html
/vagrant/scripts/services/keystone/install.sh


# Configure
# https://docs.openstack.org/keystone/train/install/keystone-users-rdo.html
/vagrant/scripts/services/keystone/configure.sh

# Verify
# https://docs.openstack.org/keystone/train/install/keystone-verify-rdo.html
/vagrant/scripts/services/keystone/verify.sh

# Create auth scripts under /root/
# https://docs.openstack.org/keystone/train/install/keystone-openrc-rdo.html
/vagrant/scripts/services/keystone/create-auth-scripts.sh

# Use auth
. /root/admin-openrc
. /root/demo-openrc
openstack token issue
```

## glance


| 信息         | 值                 | 需要修改的地方                                                                               |
| ------------ | ------------------ | -------------------------------------------------------------------------------------------- |
| 数据库密码   | `GLANCE_DBPASS`    | `/etc/glance/glance-api.conf`, 数据库GRANT指令, `glance-api.conf`                            |
| 用户名和密码 | `glance`和`glance` | openstack user create时，`/etc/glance/glance-api.conf`里`[keystone_authtoken]`下的`password` |

```bash

# Install
# https://docs.openstack.org/glance/train/install/install-rdo.html
/vagrant/scripts/services/glance/install.sh

# Verify
# https://docs.openstack.org/glance/train/install/verify.html
/vagrant/scripts/services/glance/verify.sh

```

## placement

| 信息         | 值                       | 需要修改的地方                                                                               |
| ------------ | ------------------------ | -------------------------------------------------------------------------------------------- |
| 数据库密码   | `PLACEMENT_DBPASS`       | `/etc/placement/placement-api.conf`, 数据库GRANT指令, `placement-api.conf`                   |
| 用户名和密码 | `placement`和`placement` | openstack user create时，`/etc/glance/glance-api.conf`里`[keystone_authtoken]`下的`password` |

```bash

# Install
# https://docs.openstack.org/placement/train/install/install-rdo.html
/vagrant/scripts/services/placement/install.sh

# Verify
# https://docs.openstack.org/placement/train/install/verify.html
/vagrant/scripts/services/placement/verify.sh

```

## nova

Controller

| 信息         | 值             | 需要修改的地方                                                                               |
| ------------ | -------------- | -------------------------------------------------------------------------------------------- |
| 数据库密码   | `NOVA_DBPASS`  | `/etc//placement-api.conf`, 数据库GRANT指令, `placement-api.conf`                            |
| 用户名和密码 | `nova`和`nova` | openstack user create时，`/etc/glance/glance-api.conf`里`[keystone_authtoken]`下的`password` |
| RABBIT密码   | `RABBIT_PASS`  | `placement-api.conf`的`[DEFAULT]`下的`[transport_url]`                                       |


```bash
# Install 
# https://docs.openstack.org/nova/train/install/controller-install-obs.html
# nova的配置是在https://docs.openstack.org/neutron/train/install/controller-install-rdo.html#neutron-controller-metadata-agent-rdo
/vagrant/scripts/services/nova/install-controller.sh

# 每次新增compute节点后，需要在controller上运行
su -s /bin/sh -c "nova-manage cell_v2 discover_hosts --verbose" nova

```

## neutron

Controller

| 信息         | 值                   | 需要修改的地方                                                                               |
| ------------ | -------------------- | -------------------------------------------------------------------------------------------- |
| 数据库密码   | `NEUTRON_DBPASS`     | `neutron.conf`下`[database].connection`, 数据库GRANT指令,                                    |
| 用户名和密码 | `neutron`和`neutron` | openstack user create时，`neutron.conf`里`[keystone_authtoken]`下的`password` |
| RABBIT密码   | `RABBIT_PASS`        | `neutron.conf`的`[DEFAULT]`下的`[transport_url]`                                             |


```bash
# Install 
# https://docs.openstack.org/nova/train/install/controller-install-obs.html
/vagrant/scripts/services/neutron/controller/install.sh


```
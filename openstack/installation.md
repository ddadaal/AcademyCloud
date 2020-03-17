# 安装和部署OpenStack

- `vagrant`和`virtualbox`来管理和部署虚拟机和网络
- 使用`virtualbox internal network`实现多机之间的连接
  - 主机：192.168.56.1，默认
  - Controller Node: 192.168.56.2
  - Compute Node: 192.168.56.3
  - Block storage node: 192.168.56.4
  - Provider interface: eth1
    - 修改方法：搜索`provider:eth1`，有两处neutron的要改
  - Management interface: eth2
- `bento/centos-7`镜像
    - `ubuntu 18.04`的仓库中不存在`placement-api`，自己使用pypi安装+apache2部署，会在访问终结点的时候出现403错误，无法解决
    - `generic/centos7`好像不支持root登录，没试过
    - `bento/centos-7`可以root登录，但是没有vim，可以用vi


# 下载cirros

在验证glance的过程中会从github上下cirros的镜像，为了防止速度过慢，请先把cirros下到本地`scripts`目录，在验证时将会直接使用本地的镜像。若本地镜像不存在，脚本将自动重新下载。

# Vagrant的配置

需要安装两个插件
```
vagrant plugin install vagrant-hostmanager
vagrant plugin install vagrant-vbguest
```

# Controller Node

网络已经预先配置好。eth1为provider网络。详情看[配置网络](#network)。

需要安装的包：
- [基础包](#openstack)
- [数据库](#database)
- [消息队列](#rabbitmq)
- [memcached](#memcached)
- [etcd](#etcd)


需要安装以下服务：
- Identity ([keystone](#keystone))
- Image ([glance](#glance))
- Placement ([placement](#placement))
- Compute ([nova](#nova), management portion)
- Networking ([neutron](#neutron) portion, various network agents)
- Dashboard ([horizon](#horizon))

```bash
# 一键安装
# 在安装之前，先查看各个机器的IP地址和网卡配置
/vagrant/scripts/init-controller.sh
```

# Compute Node

网络已经预先配置好。eth1为provider网络。详情看[配置网络](#network)。

包需要安装和配置：
- [基础包](#openstack)

需要安装服务：
- Compute ([nova](#nova), hypervisor portion)
- a Network service agent

```bash
# 一键安装
# 注意在多台compute上安装时改IP
# 可以搜索192.168.56.3看看哪些地方用到了compute的IP
/vagrant/scripts/init-compute.sh
```

# 安装脚本

## network

网络已经配置好。

默认的话，provider口为均为eth1。`init.sh`已经根据以下教程配置好网络。

controller：

https://docs.openstack.org/install-guide/environment-networking-controller.html

compute:

https://docs.openstack.org/install-guide/environment-networking-compute.html

block-storage不需要手动配.

## openstack packages

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
| 用户名和密码 | openstack, RABBIT_PASS | install.sh   |

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


| 信息         | 值                   | 需要修改的地方                                                                |
| ------------ | -------------------- | ----------------------------------------------------------------------------- |
| 数据库密码   | `NEUTRON_DBPASS`     | `neutron.conf`下`[database].connection`, 数据库GRANT指令,                     |
| 用户名和密码 | `neutron`和`neutron` | openstack user create时，`neutron.conf`里`[keystone_authtoken]`下的`password` |
| RABBIT密码   | `RABBIT_PASS`        | `neutron.conf`的`[DEFAULT]`下的`[transport_url]`                              |


```bash
# Controller
# Install 
# https://docs.openstack.org/nova/train/install/controller-install-obs.html
/vagrant/scripts/services/neutron/controller/install.sh

# Verify
/vagrant/scripts/services/neutron/controller/verify.sh

# Compute
/vagrant/scripts/services/neutron/compute/install.sh
```

## horizon

```bash
# Install 
# https://docs.openstack.org/horizon/train/install/install-rdo.html
/vagrant/scripts/services/horizon/install.sh
```

## cinder

```bash

# Controller
# https://docs.openstack.org/cinder/train/install/cinder-controller-install-rdo.html
/vagrant/scripts/services/cinder/controller/install.sh

# Block storage node
# https://docs.openstack.org/cinder/train/install/cinder-storage-install-rdo.html
/vagrant/scripts/services/cinder/block-storage/install.sh

# Didn't install backup
# https://docs.openstack.org/cinder/train/install/cinder-backup-install-rdo.html

# Verify on Controller
# https://docs.openstack.org/cinder/train/install/cinder-verify.html
/vagrant/scripts/services/cinder/controller/verify.sh
```


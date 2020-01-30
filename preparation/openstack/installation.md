# 安装和部署OpenStack

- `vagrant`和`virtualbox`来管理和部署虚拟机和网络
- 使用`virtualbox internal network`实现多机之间的连接
  - 主机：10.0.2.2，需要在网络连接里手动设置
  - Controller Node: 10.0.2.3
  - Compute Node: 10.0.2.4
  - 使用`vagrant-proxyconf`为各个虚拟机设置代理到主机的SS服务器`http://10.0.2.2:1080`
- `ubuntu server 18.04`镜像，[TUNA镜像](https://mirrors.tuna.tsinghua.edu.cn/ubuntu-cloud-images/bionic/)

## Controller Node

包需要安装和配置：
- 基础包
- 数据库（https://docs.openstack.org/install-guide/environment-sql-database.html）
- 消息队列（https://docs.openstack.org/install-guide/environment-messaging-ubuntu.html）
- memcached（https://docs.openstack.org/install-guide/environment-memcached-ubuntu.html）
- etcd（https://docs.openstack.org/install-guide/environment-etcd-ubuntu.html）

需要安装以下服务：
- Identity (keystone)
- Image (glance)
- Placement (placement)
- Compute
- Networking
- Dashboard (horizon)

## Compute Node

包需要安装和配置：
- 基础包

需要安装服务：
- Compute (hypervisor portion)
- Network service agent
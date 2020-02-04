# Tips

- 通过vagrant设置hostname

```ruby
config.vm.hostname = :controller
```

- bash获取脚本所在的路径

```bash
SH_DIR=$(dirname "$BASH_SOURCE")
```

- 查看keystone的日志

```bash
tail -10 /var/log/keystone/keystone-wsgi-public.log
```

<!-- Vagrant up时在涉及到网络操作的时候非常慢：

去设备管理器里删除多余的VirtualBox网卡 -->

- 不要通过往`.bashrc`里写`sudo su -`来自动切换到root用户，这样会造成所有SSH卡死，包括以后的`vagrant up`！


- 对Train版本，Ubuntu 18.04缺少包`placement-api`（这个包值在19.10上有），所以得自己用pypi下然后自己配apache，我失败了
  - https://packages.ubuntu.com/search?suite=default&section=all&arch=any&keywords=placement-api&searchon=names:w

- CentOS 7: 
  - yum上有train版所有需要的包
  - 没有vim，需要用vi
  - 需要关闭`firewalld`和`NetworkManager`服务
    - neutron暂不支持NetworkManager和firewalld
    - 开启后其他机器无法连接到controller的rabbitmq
  - 需要在**安装neutron前**按照https://docs.openstack.org/install-guide/environment-networking.html设置网络，特别是Provider网络
    - 不然会有`KeyError: "gateway"`的错误
  - NetworkManager会替代network服务，network配置lo接口后关闭network
    - 关闭NetworkManager，配置provider接口后需要手动启动network服务

```bash

systemctl stop NetworkManager firewalld
systemctl disable NetworkManager firewalld

# 下面两句可以写成run: always的shell provision
# 假设scripts/network/ifcfg-eth1是provider网络的配置文件
cp /vagrant/scripts/network/ifcfg-eth1 /etc/sysconfig/network-scripts/
# 如果用vagrant的public_network让它自动配置，那么就得每次手动重启network
systemctl restart network
```

ceilometer依赖的gnocchi依赖uwsgi，得yum手动安装
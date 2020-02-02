. /root/admin-openrc

# Provider network

openstack network create  --share --external \
  --provider-physical-network provider \
  --provider-network-type flat provider

# start和end的子网需要对应public_network，也就是现实中路由器所在的子网，并保证这中间的IP没有被分配
# gateway可以通过查看ip route查看provider interface（本项目中为eth1）的值，一般来说是路由器的IP
# dns-nameserver应该也是路由器的IP，
#   看/etc/resolv.conf里有两个，但是有一个是10.0.2.0/24子网下的
openstack subnet create --network provider \
  --allocation-pool start=10.0.0.101,end=10.0.0.250 \
  --dns-nameserver 10.0.0.1 --gateway 10.0.0.1 \
  --subnet-range 10.0.0.0/24 provider
  
# Self-service network
. /root/demo-openrc

openstack network create selfservice

# dns-nameserver和上面一样，一般是路由器IP
# gateway和subnet-range都是自己定的，保证在一个子网里，确保是私有IP地址空间
# 这里选用172.16.1.0
# 可以看https://tools.ietf.org/html/rfc1918的3. Private Address Space部分
openstack subnet create --network selfservice \
  --dns-nameserver 10.0.0.1 --gateway 172.16.1.1 \
  --subnet-range 172.16.1.0/24 selfservice

openstack router create router
openstack router add subnet router selfservice
openstack router set router --external-gateway provider
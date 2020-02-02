openstack flavor create --id 0 --vcpus 1 --ram 64 --disk 1 m1.nano

. /root/demo-openrc
ssh-keygen -q -N ""
openstack keypair create --public-key ~/.ssh/id_rsa.pub mykey
openstack keypair list

openstack security group rule create --proto icmp default
openstack security group rule create --proto tcp --dst-port 22 default

# Launch instance
# https://docs.openstack.org/install-guide/launch-instance-selfservice.html

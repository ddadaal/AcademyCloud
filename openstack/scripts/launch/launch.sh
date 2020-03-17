# add flavors
openstack flavor create --id 1 m1.tiny --vcpus 1 --ram 64 --disk 1 --public
openstack flavor create --id 2 m1.small --vcpus 1 --ram 2048 --disk 20 --public
openstack flavor create --id 3 m1.medium --vcpus 2 --ram 4096 --disk 40 --public
openstack flavor create --id 4 m1.large --vcpus 4 --ram 8192 --disk 80 --public

# . /root/demo-openrc
# ssh-keygen -q -N ""
# openstack keypair create --public-key ~/.ssh/id_rsa.pub mykey
# openstack keypair list

openstack security group rule create --proto icmp default
openstack security group rule create --proto tcp --dst-port 22 default

# Launch instance
# https://docs.openstack.org/install-guide/launch-instance-selfservice.html

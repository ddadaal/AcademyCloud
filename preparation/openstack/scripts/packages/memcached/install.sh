# https://docs.openstack.org/install-guide/environment-memcached-ubuntu.html

yum install -y memcached python-memcached
sed -i "s/OPTIONS=\"-l 127.0.0.1,::1/OPTIONS=\"-l 127.0.0.1,::1,controller/" /etc/sysconfig/memcached 
systemctl enable memcached.service
systemctl start memcached.service
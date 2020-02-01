SH_DIR=$(dirname "$BASH_SOURCE")

mysql -uroot -e "CREATE DATABASE neutron;"
mysql -uroot -e "GRANT ALL PRIVILEGES ON neutron.* TO 'neutron'@'localhost' \
  IDENTIFIED BY 'NEUTRON_DBPASS';"
mysql -uroot -e "GRANT ALL PRIVILEGES ON neutron.* TO 'neutron'@'%' \
  IDENTIFIED BY 'NEUTRON_DBPASS';"

. /root/admin-openrc

openstack user create --domain default --password neutron neutron
openstack role add --project service --user neutron admin
openstack service create --name neutron \
  --description "OpenStack Networking" network
openstack endpoint create --region RegionOne \
  network public http://controller:9696
openstack endpoint create --region RegionOne \
  network internal http://controller:9696
openstack endpoint create --region RegionOne \
  network admin http://controller:9696

# Set Option 2 Self-service networks
# https://docs.openstack.org/neutron/train/install/controller-install-option2-rdo.html
yum install openstack-neutron openstack-neutron-ml2 \
  openstack-neutron-linuxbridge ebtables

cp "$SH_DIR/neutron.conf" /etc/neutron/neutron.conf
cp "$SH_DIR/ml2_config.ini" /etc/neutron/plugins/ml2/ml2_conf.ini
cp "$SH_DIR/linuxbridge_agent.ini" /etc/neutron/plugins/ml2/linuxbridge_agent.ini

modprobe br_netfilter
cp "$SH_DIR/sysctl.conf" /etc/sysctl.conf
sysctl -p

cp "$SH_DIR/l3_agent.ini" /etc/neutron/l3_agent.ini
cp "$SH_DIR/dhcp_agent.ini" /etc/neutron/dhcp_agent.ini

# Configure the metadata agent
# https://docs.openstack.org/neutron/train/install/controller-install-rdo.html#neutron-controller-metadata-agent-rdo

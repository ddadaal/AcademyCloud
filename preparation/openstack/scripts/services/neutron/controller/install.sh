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
yum install -y openstack-neutron openstack-neutron-ml2 \
  openstack-neutron-linuxbridge ebtables

cp "$SH_DIR/neutron.conf" /etc/neutron/neutron.conf
cp "$SH_DIR/ml2_conf.ini" /etc/neutron/plugins/ml2/ml2_conf.ini
cp "$SH_DIR/linuxbridge_agent.ini" /etc/neutron/plugins/ml2/linuxbridge_agent.ini

# Enable bridge
modprobe br_netfilter
cp "$SH_DIR/sysctl.conf" /etc/sysctl.conf
sysctl -p

cp "$SH_DIR/l3_agent.ini" /etc/neutron/l3_agent.ini
cp "$SH_DIR/dhcp_agent.ini" /etc/neutron/dhcp_agent.ini

# Configure the metadata agent
# https://docs.openstack.org/neutron/train/install/controller-install-rdo.html#neutron-controller-metadata-agent-rdo
cp "$SH_DIR/metadata_agent.ini" /etc/neutron/metadata_agent.ini

ln -s /etc/neutron/plugins/ml2/ml2_conf.ini /etc/neutron/plugin.ini
su -s /bin/sh -c "neutron-db-manage --config-file /etc/neutron/neutron.conf \
  --config-file /etc/neutron/plugins/ml2/ml2_conf.ini upgrade head" neutron
systemctl restart openstack-nova-api.service
systemctl enable neutron-server.service \
  neutron-linuxbridge-agent.service neutron-dhcp-agent.service \
  neutron-metadata-agent.service
systemctl start neutron-server.service \
  neutron-linuxbridge-agent.service neutron-dhcp-agent.service \
  neutron-metadata-agent.service
systemctl enable neutron-l3-agent.service
systemctl start neutron-l3-agent.service
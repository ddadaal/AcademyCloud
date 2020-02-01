SH_DIR=$(dirname "$BASH_SOURCE")

yum install -y openstack-neutron-linuxbridge ebtables ipset

cp "$SH_DIR/neutron.conf" /etc/neutron/neutron.conf
cp "$SH_DIR/linuxbridge_agent.ini" /etc/neutron/plugins/ml2/linuxbridge_agent.ini

# Enable bridge
modprobe br_netfilter
cp "$SH_DIR/sysctl.conf" /etc/sysctl.conf
sysctl -p

systemctl restart openstack-nova-compute.service
systemctl enable neutron-linuxbridge-agent.service
systemctl start neutron-linuxbridge-agent.service
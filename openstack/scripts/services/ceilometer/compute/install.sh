# No IPMI.
# To configure IPMI, see https://docs.openstack.org/ceilometer/latest/install/install-compute-rdo.html

SH_DIR=$(dirname "$BASH_SOURCE")
yum install -y openstack-ceilometer-compute

cp "$SH_DIR/ceilometer.conf" /etc/ceilometer/

systemctl enable openstack-ceilometer-compute.service
systemctl start openstack-ceilometer-compute.service
systemctl restart openstack-nova-compute.service
SH_DIR=$(dirname "$BASH_SOURCE")
yum install -y openstack-nova-compute

cp "$SH_DIR/nova.conf" /etc/nova/nova.conf
egrep -c '(vmx|svm)' /proc/cpuinfo
systemctl enable libvirtd.service openstack-nova-compute.service
systemctl start libvirtd.service openstack-nova-compute.service
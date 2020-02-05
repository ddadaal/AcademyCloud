SH_DIR=$(dirname "$BASH_SOURCE")

# Install and configure packages
"$SH_DIR/packages/openstack/install.sh"
"$SH_DIR/packages/database/install.sh"
"$SH_DIR/packages/rabbitmq/install.sh"
"$SH_DIR/packages/memcached/install.sh"
"$SH_DIR/packages/etcd/install.sh"

# Install services

# keystone
"$SH_DIR/services/keystone/install.sh"
"$SH_DIR/services/keystone/configure.sh"
"$SH_DIR/services/keystone/verify.sh"
"$SH_DIR/services/keystone/create-auth-scripts.sh"

# glance
"$SH_DIR/services/glance/install.sh"
"$SH_DIR/services/glance/verify.sh"

# placement
"$SH_DIR/services/placement/install.sh"
"$SH_DIR/services/placement/verify.sh"

# nova
"$SH_DIR/services/nova/controller/install.sh"
read -p "Configure nova on a compute node and then press to continue." R

. /root/admin-openrc
openstack compute service list --service nova-compute
su -s /bin/sh -c "nova-manage cell_v2 discover_hosts --verbose" nova
"$SH_DIR/services/nova/controller/verify.sh"

# neutron
"$SH_DIR/services/neutron/controller/install.sh"
read -p "Configure neutron on a compute node and then press to continue" R
"$SH_DIR/services/neutron/controller/verify.sh"

# horizon
"$SH_DIR/services/horizon/install.sh"

# cinder
"$SH_DIR/services/cinder/controller/install.sh"
read -p "Configure cinder on block-storage node and then press to continue" R
"$SH_DIR/services/cinder/controller/verify.sh"

# ceilometer
"$SH_DIR/services/ceilometer/controller/install.sh"
read -p "Configure ceilometer on compute node and then press to continue" R

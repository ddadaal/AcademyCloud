SH_DIR=$(dirname "$BASH_SOURCE")

# Install and configure packages
read -p "Install environments (Y/n)?" R
if [ "$R" != "n" ] ; then
    "$SH_DIR/packages/openstack/install.sh"
    "$SH_DIR/packages/database/install.sh"
    "$SH_DIR/packages/rabbitmq/install.sh"
    "$SH_DIR/packages/memcached/install.sh"
    "$SH_DIR/packages/etcd/install.sh"
fi

# Install services

read -p "Install keystone (Y/n)?" R
if [ "$R" != "n" ] ; then
    "$SH_DIR/services/keystone/install.sh"
    "$SH_DIR/services/keystone/configure.sh"
    "$SH_DIR/services/keystone/verify.sh"
    "$SH_DIR/services/keystone/create-auth-scripts.sh"
fi

read -p "Install glance? (Y/n)" R
if [ "$R" != "n" ] ; then
    "$SH_DIR/services/glance/install.sh"
    "$SH_DIR/services/glance/verify.sh"
fi

read -p "Install placement? (Y/n)" R
if [ "$R" != "n" ] ; then
    "$SH_DIR/services/placement/install.sh"
    "$SH_DIR/services/placement/verify.sh"
fi


read -p "Install nova on controller? (Y/n)" R
if [ "$R" != "n" ] ; then
    "$SH_DIR/services/nova/controller/install.sh"
    "$SH_DIR/services/placement/verify.sh"
fi
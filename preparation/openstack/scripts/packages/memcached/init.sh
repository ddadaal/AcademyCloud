# https://docs.openstack.org/install-guide/environment-memcached-ubuntu.html
SH_DIR=$(dirname "$BASH_SOURCE")

apt install -y memcached python-memcache
cp "$SH_DIR/memcached.conf" /etc/memcached.conf
service memcached restart
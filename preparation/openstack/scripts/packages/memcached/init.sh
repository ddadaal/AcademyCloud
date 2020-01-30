# https://docs.openstack.org/install-guide/environment-memcached-ubuntu.html
SH_DIR=$(dirname "$BASH_SOURCE")

apt install memcached python-memcache
cat "$SH_DIR/memcached.conf" > /etc/memcached.conf
service memcached restart
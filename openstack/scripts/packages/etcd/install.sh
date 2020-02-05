# https://docs.openstack.org/install-guide/environment-etcd-ubuntu.html
SH_DIR=$(dirname "$BASH_SOURCE")
yum install -y etcd
cp "$SH_DIR/etcd.conf" /etc/etcd/
systemctl enable etcd
systemctl start etcd
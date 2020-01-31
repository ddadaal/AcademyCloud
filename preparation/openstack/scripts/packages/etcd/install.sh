# https://docs.openstack.org/install-guide/environment-etcd-ubuntu.html
SH_DIR=$(dirname "$BASH_SOURCE")
yum install -y etcd
cp "$BASH_SOURCE/etcd.conf" /etc/etcd/
systemctl enable etcd
systemctl start etcd
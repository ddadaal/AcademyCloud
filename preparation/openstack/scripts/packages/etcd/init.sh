# https://docs.openstack.org/install-guide/environment-etcd-ubuntu.html
SH_DIR=$(dirname "$BASH_SOURCE")
apt install -y etcd
cp "$BASH_SOURCE/etcd" /etc/default/etcd
systemctl enable etcd
systemctl restart etcd
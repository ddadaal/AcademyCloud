SH_DIR=$(dirname "$BASH_SOURCE")

yum install -y mariadb mariadb-server python2-PyMySQL
cp "$SH_DIR/openstack.cnf" /etc/my.cnf.d/
systemctl enable mariadb.service
systemctl start mariadb.service
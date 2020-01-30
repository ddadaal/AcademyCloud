# https://docs.openstack.org/install-guide/environment-sql-database.html
SH_DIR=$(dirname "$BASH_SOURCE")

apt install -y mariadb-server python-pymysql
cp "$SH_DIR/99-openstack.cnf" /etc/mysql/mariadb.conf.d/99-openstack.cnf
service mysql restart
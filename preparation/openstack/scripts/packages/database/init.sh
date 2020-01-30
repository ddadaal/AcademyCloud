# https://docs.openstack.org/install-guide/environment-sql-database.html
SH_DIR=$(dirname "$BASH_SOURCE")

apt install mariadb-server python-pymysql
cat "$SH_DIR/99-openstack.cnf" > /etc/mysql/mariadb.conf.d/99-openstack.cnf
service mysql restart
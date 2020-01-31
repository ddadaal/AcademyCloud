SH_DIR=$(dirname "$BASH_SOURCE")

mysql -uroot -e "CREATE DATABASE placement;"
mysql -uroot -e "GRANT ALL PRIVILEGES ON placement.* TO 'placement'@'localhost' IDENTIFIED BY 'PLACEMENT_DBPASS';"
mysql -uroot -e "GRANT ALL PRIVILEGES ON placement.* TO 'placement'@'%' IDENTIFIED BY 'PLACEMENT_DBPASS';"

. /root/admin-openrc

openstack user create --domain default --password placement placement
openstack role add --project service --user placement admin
openstack service create --name placement --description "Placement API" placement
openstack endpoint create --region RegionOne placement public http://controller:8778
openstack endpoint create --region RegionOne placement internal http://controller:8778
openstack endpoint create --region RegionOne placement admin http://controller:8778

# Install placement through pip

pip3 install openstack-placement pymysql
mkdir /etc/placement
cp "$SH_DIR/placement.conf" /etc/placement/
placement-manage db sync

# Configure Apache2

cp "$SH_DIR/apache-placement.conf" /etc/apache2/sites-available/placement.conf
ln -s /etc/apache2/sites-available/placement.conf /etc/apache2/sites-enabled/placement.conf
service apache2 restart

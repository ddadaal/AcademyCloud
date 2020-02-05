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

yum install -y openstack-placement-api
cp "$SH_DIR/placement.conf" /etc/placement/
su -s /bin/sh -c "placement-manage db sync" placement

# https://ask.openstack.org/en/question/122022/stein-expecting-value-line-1-column-1-char-0-oscplacement/
cp "$SH_DIR/00-placement-api.conf" /etc/httpd/conf.d/

systemctl restart httpd
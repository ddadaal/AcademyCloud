SH_DIR=$(dirname "$BASH_SOURCE")
mysql -uroot -e "CREATE DATABASE cinder;"
mysql -uroot -e "GRANT ALL PRIVILEGES ON cinder.* TO 'cinder'@'localhost' IDENTIFIED BY 'CINDER_DBPASS';"
mysql -uroot -e "GRANT ALL PRIVILEGES ON cinder.* TO 'cinder'@'%' IDENTIFIED BY 'CINDER_DBPASS';"

. /root/admin-openrc

openstack user create --domain default --password cinder cinder
openstack role add --project service --user cinder admin
openstack service create --name cinderv2 \
  --description "OpenStack Block Storage" volumev2
openstack service create --name cinderv3 \
  --description "OpenStack Block Storage" volumev3
openstack endpoint create --region RegionOne \
  volumev2 public http://controller:8776/v2/%\(project_id\)s
openstack endpoint create --region RegionOne \
  volumev2 internal http://controller:8776/v2/%\(project_id\)s
openstack endpoint create --region RegionOne \
  volumev2 admin http://controller:8776/v2/%\(project_id\)s

openstack endpoint create --region RegionOne \
  volumev3 public http://controller:8776/v3/%\(project_id\)s
openstack endpoint create --region RegionOne \
  volumev3 internal http://controller:8776/v3/%\(project_id\)s
openstack endpoint create --region RegionOne \
  volumev3 admin http://controller:8776/v3/%\(project_id\)s

yum install -y openstack-cinder

cp "$SH_DIR/cinder.conf" /etc/cinder/
su -s /bin/sh -c "cinder-manage db sync" cinder

cp "$SH_DIR/nova.conf" /etc/nova/

systemctl restart openstack-nova-api.service
systemctl enable openstack-cinder-api.service openstack-cinder-scheduler.service
systemctl start openstack-cinder-api.service openstack-cinder-scheduler.service
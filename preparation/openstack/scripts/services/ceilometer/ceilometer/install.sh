SH_DIR=$(dirname "$BASH_SOURCE")

"$SH_DIR/environ.sh"

. /root/admin-openrc
openstack user create --domain default --password ceilometer ceilometer
openstack role add --project service --user ceilometer admin

openstack user create --domain default --password gnocchi gnocchi
openstack service create --name gnocchi \
    --description "Metric Service" metric
openstack role add --project service --user gnocchi admin

openstack endpoint create --region RegionOne \
    metric public http://controller:8041
openstack endpoint create --region RegionOne \
    metric internal http://controller:8041
openstack endpoint create --region RegionOne \
    metric admin http://controller:8041

yum install -y openstack-gnocchi-api openstack-gnocchi-metricd \
    python-gnocchiclient

mysql -uroot -e "CREATE DATABASE gnocchi;"
mysql -uroot -e "GRANT ALL PRIVILEGES ON gnocchi.* TO 'gnocchi'@'localhost' IDENTIFIED BY 'GNOCCHI_DBPASS';"
mysql -uroot -e "GRANT ALL PRIVILEGES ON gnocchi.* TO 'gnocchi'@'%' IDENTIFIED BY 'GNOCCHI_DBPASS';"

cp "$SH_DIR/gnocchi.conf" /etc/gnocchi/

# Enable redis
yum install -y redis
systemctl enable redis
systemctl start redis

gnocchi-upgrade

systemctl enable openstack-gnocchi-api.service \
    openstack-gnocchi-metricd.service
systemctl start openstack-gnocchi-api.service \
    openstack-gnocchi-metricd.service

yum install -y openstack-ceilometer-notification \
    openstack-ceilometer-central

cp "$SH_DIR/ceilometer.conf" /etc/ceilometer/
cp "$SH_DIR/pipeline.yaml" /etc/ceilometer/

ceilometer-upgrade

systemctl enable openstack-ceilometer-notification.service \
    openstack-ceilometer-central.service
systemctl start openstack-ceilometer-notification.service \
    openstack-ceilometer-central.service

echo "Ceilometer installed. Configure component with https://docs.openstack.org/ceilometer/latest/install/install-controller.html#ceilometer"

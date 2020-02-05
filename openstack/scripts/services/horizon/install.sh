SH_DIR=$(dirname "$BASH_SOURCE")

yum install -y openstack-dashboard
cp "$SH_DIR/local_settings" /etc/openstack-dashboard/local_settings
cp "$SH_DIR/openstack-dashboard.conf" /etc/httpd/conf.d/

systemctl restart httpd.service memcached.service
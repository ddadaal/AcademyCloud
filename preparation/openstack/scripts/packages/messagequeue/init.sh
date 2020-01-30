# https://docs.openstack.org/install-guide/environment-messaging-ubuntu.html
apt install rabbitmq-server
rabbitmqctl add_user openstack RABBIT_PASS
rabbitmqctl set_permissions openstack ".*" ".*" ".*"
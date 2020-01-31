. /root/admin-openrc
placement-status upgrade check

pip3 install --ignore-installed simplejson
pip3 install osc-placement

openstack --os-placement-api-version 1.2 resource class list --sort-column name
openstack --os-placement-api-version 1.6 trait list --sort-column name
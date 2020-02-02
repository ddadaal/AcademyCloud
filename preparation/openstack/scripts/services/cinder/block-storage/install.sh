SH_DIR=$(dirname "$BASH_SOURCE")
pvcreate /dev/sdb
vgcreate cinder-volumes /dev/sdb
cp "$SH_DIR/lvm.conf" /etc/lvm/

echo "Add the following filter in /etc/lvm/lvm.conf of all compute nodes"
echo "devices {"
echo "  ..."
echo "  filter = [ \"a|/dev/sda|\", \"r|.*/|\" ]"
echo "}"
read -p "When complete, press any key to continue" R

yum install -y openstack-cinder targetcli python-keystone
cp "$SH_DIR/cinder.conf" /etc/cinder/

systemctl enable openstack-cinder-volume.service target.service
systemctl start openstack-cinder-volume.service target.service
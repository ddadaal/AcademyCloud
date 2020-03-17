cd /root
. /root/admin-openrc

imgname="cirros-0.4.0-x86_64-disk.img"

if [ -f "/vagrant/$imgname" ]; then
  echo "Use existing img"
  cp "/vagrant/$imgname" /root
else
  echo "local image not exists. Downloading from github..."
  wget http://download.cirros-cloud.net/0.4.0/cirros-0.4.0-x86_64-disk.img
fi

glance image-create --name "cirros" \
  --file cirros-0.4.0-x86_64-disk.img \
  --disk-format qcow2 --container-format bare \
  --visibility public
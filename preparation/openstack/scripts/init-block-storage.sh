SH_DIR=$(dirname "$BASH_SOURCE")

"$SH_DIR/packages/openstack/install.sh"

read -p "Ready to install cinder?" R
"$SH_DIR/services/cinder/block-storage/install.sh"
read -p "cinder installed. Verify cinder on the controller and press to exit." R

SH_DIR=$(dirname "$BASH_SOURCE")

"$SH_DIR/packages/openstack/install.sh"

"$SH_DIR/services/cinder/block-storage/install.sh"

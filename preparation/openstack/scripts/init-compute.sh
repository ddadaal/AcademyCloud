SH_DIR=$(dirname "$BASH_SOURCE")

# Install environment
"$SH_DIR/packages/openstack/install.sh"

# Install nova
"$SH_DIR/services/nova/compute/install.sh"
read -p "Verify nova on controller, and press to continue, or n to cancel. (Y/n)" R
if [ "$R" = "n" ]; then
    exit 0
fi

# Install neutron
"$SH_DIR/services/neutron/compute/install.sh"
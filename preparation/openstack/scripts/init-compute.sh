SH_DIR=$(dirname "$BASH_SOURCE")

# Install environment
"$SH_DIR/packages/openstack/install.sh"

# Install nova
read -p "Ready to install nova?" R
"$SH_DIR/services/nova/compute/install.sh"
read -p "Verify nova on controller, and press to continue to install neutron." R

# Install neutron
"$SH_DIR/services/neutron/compute/install.sh"
read -p "Verify neutron on controller, and press to continue to install neutron." R
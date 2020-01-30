SH_DIR=$(dirname "$BASH_SOURCE")
hostnamectl set-hostname controller
cat "$SH_DIR/hosts" >> /etc/hosts
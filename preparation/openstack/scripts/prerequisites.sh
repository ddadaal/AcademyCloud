
echo "Set tuna as apt mirror"
sudo mv /tmp/sources.list /etc/apt/sources.list

echo "Update apt and install openstackclient"
sudo apt update
sudo apt install -y python3-openstackclient

echo "Add automatic switch to root"
echo "sudo su -" >> .bashrc
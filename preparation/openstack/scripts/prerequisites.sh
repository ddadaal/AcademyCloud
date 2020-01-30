echo "Set tuna as apt mirror"
sudo cp /etc/apt/sources.list /etc/apt/sources.list.bk
sudo cat /vagrant/scripts/sources.list > /etc/apt/sources.list

echo "Update apt and install openstackclient"
sudo apt update
sudo apt install -y python3-openstackclient

echo "Add automatic switch to root"
echo "sudo su -" >> .bashrc
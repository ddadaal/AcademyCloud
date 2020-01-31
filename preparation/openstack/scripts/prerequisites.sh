
echo "Set tuna as apt mirror"
sudo mv /tmp/sources.list /etc/apt/sources.list

echo "Update apt and install openstackclient"
sudo apt update
sudo apt install -y python3-openstackclient

echo "Install pip3 and set mirror"
sudo curl "https://bootstrap.pypa.io/get-pip.py" -o "get-pip.py"
sudo apt-get install python3-distutils
sudo python3 get-pip.py --user
ln -s /root/.local/bin/pip3 /usr/bin/pip3
pip3 config set global.index-url https://pypi.tuna.tsinghua.edu.cn/simple

echo "Add automatic switch to root"
echo "sudo su -" >> .bashrc
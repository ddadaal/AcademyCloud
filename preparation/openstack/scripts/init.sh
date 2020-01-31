
echo "Set tuna as yum mirror"
sudo mv /tmp/CentOS-Base.repo /etc/yum.repos.d/
sed -i 's/enabled=1/enabled=0/' /etc/yum/pluginconf.d/fastestmirror.conf
sudo yum makecache

# echo "Install pip3 and set mirror"
# sudo curl "https://bootstrap.pypa.io/get-pip.py" -o "get-pip.py"
# sudo yum install -y python3-distutils
# sudo python3 get-pip.py --user
# sudo ln -s /root/.local/bin/pip3 /usr/bin/pip3
# sudo pip3 config set global.index-url https://pypi.tuna.tsinghua.edu.cn/simple

echo "Add automatic switch to root"
echo "sudo su -" >> .bashrc
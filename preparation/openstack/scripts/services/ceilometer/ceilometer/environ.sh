echo "Setup EPEL"
yum install epel-release
sed -e 's!^metalink=!#metalink=!g' \
    -e 's!^#baseurl=!baseurl=!g' \
    -e 's!//download\.fedoraproject\.org/pub!//mirrors.tuna.tsinghua.edu.cn!g' \
    -e 's!http://mirrors\.tuna!https://mirrors.tuna!g' \
    -i /etc/yum.repos.d/epel.repo /etc/yum.repos.d/epel-testing.repo

echo "Setup pip"
yum install -y python-pip
pip install pip -U
pip config set global.index-url https://pypi.tuna.tsinghua.edu.cn/simple

echo "Install C"
yum install -y gcc python-devel

echo "Build uwsgi"
pip install uwsgi
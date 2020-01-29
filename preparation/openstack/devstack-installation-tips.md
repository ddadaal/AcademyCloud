# 安装`devstack`的注意事项

安装过程大致在按照官方教程https://docs.openstack.org/devstack/latest/就可以了，但是我自己安装的时候遇到很多坑并用了一天仍未成功，这里记一下，以下错误按时间顺序。可以通过重新运行脚本自行解决的错误、网络错误等就不在这里记录了。

# Ubuntu 18.04 Hyper-V VM


## `./stack.sh`时`Permission denied: '/opt/stack/.cache...'`

运行`./stack.sh`报错：

> PermissionError: [Errno 13] Permission denied: '/opt/stack/.cache/pip/wheels/d3/1b/eb/fc930561cad16a5c8dd5436b428e0fa2237899314418e6e8cb'

### 解决方法

删除所有文件重新开始。在运行`./stack.sh`之前，先以`stack`用户在`/opt/stack`下创建一个`.cache`目录。

### 发现过程

- 使用`ls -al ~/.cache`发现这个目录（在运行`stack.sh`过程中创建）的权限被设置成了`root:root`。
- 使用`chown`设置成了`stack:stack`之后重新`./unstack.sh`和`./clean.sh`
- 重新`./stack.sh`报错：

> "/opt/stack/requirements/.venv/bin/pip’: No such file or directory"

- 使用`virtualenv /opt/stack/requirements/.venv`问题持续，并发现`.venv`下有三个软链接，形成死循环

```
python -> python3
python3 -> python3.6
python3.6 -> python3
```

- 放弃修复，删除`/opt/stack`下所有内容，重新开始
- 想到权限问题，于是先自己创建`stack`的`.cache`目录再开始运行`stack.sh`。问题解决。

### 参考

https://bugs.launchpad.net/devstack/+bug/1671409

## 在解决上述问题之后，Cannot uninstall ‘{httplib2,simplejson}’

解决掉上述问题之后，`./stack.sh`报错：

> Cannot uninstall ‘httplib2’. It is a distutils installed project and thus we cannot accurately determine which files belong to it which would lead to only a partial uninstall.

`simplejson`包也有这个问题

### 解决方法

手动强制更新：`sudo pip install --ignore-installed httplib2 simplejson`后重新运行`stack.sh`

### 参考

https://blog.csdn.net/kudou1994/article/details/82383588

## /opt/stack/devstack/lib/glance:333 g-api did not start

出现报错

> /opt/stack/devstack/lib/glance:333 g-api did not start

错误上面出现多个

```
 +::                                        [[ 503 == 503 ]]
+::                                        sleep 1
++::                                        curl -g -k --noproxy '*' -s -o /dev/null -w '%{http_code}' http://10.0.0.9/image
```

### 解决方法

放弃了

### 发现过程

- 搜索错误提示，发现[同样的问题](https://ask.openstack.org/en/question/112030/glance354-g-api-did-not-start-during-devstack-installation/)
- 根据里面的提示，`sudo systemctl start devstack@g-api.service`手动启动服务
- 再次运行`stack.sh`同样的错误
- `journalctl -u devstack@g-api`查看日志，发现日志前三行是：

```
[uWSGI] getting INI configuration from /etc/glance/glance-uwsgi.ini
open("./python_plugin.so"): No such file or directory [core/utils.c line 3724]
!!! UNABLE to load uWSGI plugin: ./python_plugin.so: cannot open shared object file: No such file or directory !!!
```

- 把那行!!!开头的查，根据一个[StackOverflow回答](https://stackoverflow.com/questions/15936413/pip-installed-uwsgi-python-plugin-so-error)发现需要修改第一行的ini配置，注释其中的`plugin = python`
- 修改后重启服务`sudo systemctl restart devstack@g-api.service`
- 再次查看日志发现没有这一行错误
- 再次运行`stack.sh`同样的错误
- 再次查看被修改的ini文件，发现文件被改回来了
- 找不到其他有意义的解决方案，放弃


### 参考

https://ask.openstack.org/en/question/112030/glance354-g-api-did-not-start-during-devstack-installation/

https://stackoverflow.com/questions/15936413/pip-installed-uwsgi-python-plugin-so-error

# CentOS 7

## /opt/stack/devstack/functions:583 Invalid path permissions

前面提示`/opt/stack appears to have 0700 permissions. This is very likely to cause fatal issues for DevStack daemons.`

## 解决方法

修改权限为`755`：`chmod 755 /opt/stack`

## 参考

https://www.cnblogs.com/rhjeans/p/11328346.html

## Ubuntu Server 18.04 VM on Microsoft Azure

遇到了本机Hyper-V VM的前三个问题，解决后直接安装成功。
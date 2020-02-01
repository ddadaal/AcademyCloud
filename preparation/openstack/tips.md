# Tips

通过vagrant设置hostname

```ruby
config.vm.hostname = :controller
```

bash获取脚本所在的路径

```bash
SH_DIR=$(dirname "$BASH_SOURCE")
```

查看keystone的日志

```bash
tail -10 /var/log/keystone/keystone-wsgi-public.log
```

<!-- Vagrant up时在涉及到网络操作的时候非常慢：

去设备管理器里删除多余的VirtualBox网卡 -->

不要通过往`.bashrc`里写`sudo su -`来自动切换到root用户，这样会造成所有SSH卡死，包括以后的`vagrant up`！
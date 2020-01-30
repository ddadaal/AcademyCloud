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

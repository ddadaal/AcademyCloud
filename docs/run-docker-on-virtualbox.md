# 在VirtualBox上运行Docker Engine

因为OpenStack需要虚拟机，而vagrant对Hyper-V的支持比较差，所以必须需要运行VirtualBox，但是VirtualBox和Docker所需要使用的Hyper-V又不兼容，所以需要在VirtualBox上安装Docker Engine来使用Docker。

# 安装

1. 从GitHub上下载并安装docker toolbox: https://github.com/docker/toolbox/releases，在过程中不安装Git for Windows和VirtualBox（因为已经安装）
2. 为了防止`docker-machine`自己下载不经过代理过于缓慢，所以手动下载`boot2docker`镜像：https://github.com/boot2docker/boot2docker/releases/download/v19.03.5/boot2docker.iso，并放到`~/.docker/machine/cache\boot2docker.iso`位置
3. 运行`docker-machine create --driver virtualbox default`
4. 等待安装完成，完成后，可以通过`docker-machine ls`验证安装成功
5. 运行`docker-machine env default`让`docker-machine`后续操作都在目标主机上
6. 再运行`& "C:\Program Files\Docker Toolbox\docker-machine.exe" env default | Invoke-Expression`让docker使用目标主机
7. 运行`vim $PROFILE`打开PowerShell配置文件，然后将以上命令加入配置文件中，使以后PowerShell每次开启时就自动配置环境。
8. 配置完成。以后每次重启电脑，需要手动运行`docker-machine start`启动虚拟机。

# 设置代理

1. 使用`docker-machine ssh`登录上虚拟机
2. 运行`sudo vim /etc/docker/daemon.json`修改docker设置文件，并填入以下内容

```json
{
  "registry-mirrors": [
    "https://dockerhub.azk8s.cn",
    "https://gcr.azk8s.cn"
  ]
}
```

3. 运行`sudo reboot`重启虚拟机



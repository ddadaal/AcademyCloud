# Keystone权限

| 业务角色 | Keystone用户 | domain  | project | role          | 权限                                  |
| -------- | ------------ | ------- | ------- | ------------- | ------------------------------------- |
| 平台管理 | admin        | default | default | admin         | 所有权限                              |
| 学校管理 | nju-admin    | nju     | default | domain_admin  | 管理一个domain下的所有project, user等 |
| 老师     | 67           | nju     | 67      | project_admin | 管理project下的用户和资源等           |
| 同学     | cjd          | nju     | 67      | member        | 管理自己名下的资源                    |

## 相关创建命令


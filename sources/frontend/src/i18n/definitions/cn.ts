import zh_CN from "antd/es/locale/zh_CN";

export default {
  metadata: {
    id: "cn",
    langStrings: ["cn", "zh-CN", "zh"],
    detailedId: "zh-CN",
    name: "简体中文",
    antdConfigProvider: {
      locale: zh_CN,
    },
  },
  definitions: {
    validateMessages: {
      email: "请输入有效的电子邮箱地址。",
      required: "这是必需字段。",
    },
    components: {
      resources: {
        resourceType: "资源类型",
        values: "资源量",
        cpu: "CPU核心数",
        memory: "内存 (MB)",
        storage: "存储 (GB)",
      },
      users: {
        id: "ID",
        username: "用户名",
        name: "姓名",
        email: "电子邮箱",
        active: {
          title: "状态",
          true: "可用",
          false: "停用",
        },
        role: {
          title: "角色",
          admin: "管理员",
          member: "成员",
        },
        addUser: {
          button: "增加成员",
          selectUser: "选择用户（已加入的成员不显示）",
          selectRole: "选择角色",
        },
        actions: "操作",
        remove: {
          link: "移除",
          opName: "移除",
          prompt: "确定要移除吗？",
          errors: {
            payUser: "不能移除付费用户。请先将付费用户修改为他人后再尝试。",
            onlyAdmin: "不能移除唯一的管理员。请先设置其他管理员再尝试。",
          },
        },
        changeRole: {
          opName: "修改权限",
          errors: {
            payUser: "不能修改付费用户的角色。请先将付费用户修改为他人后再尝试。",
            onlyAdmin: "不能修改唯一的管理员的角色。请先设置其他管理员再尝试。",
          },
        },
      },
      operationResult: {
        inProgress: "{}中……",
        success: "{}成功！",
        fail: "{}失败",
        retry: "请重试。",
      },
    },
    homepage: {
      title: "AcademyCloud",
      description: "为大学设计的云平台",
      pageIndicator: {
        login: "登录",
        register: "注册",
      },
      loginForm: {
        title: "登录",
        username: "用户名",
        password: "密码",
        login: "登录",
        remember: "记住我",
        forget: "忘记密码",
        loginFailTitle: "登录失败",
        noScope: "您未加入任何学校或者项目！请联系管理员。",
        other: "登录失败",
      },
      registerForm: {
        title: "注册",
        username: "用户名",
        password: "密码",
        email: "电子邮箱",
        register: "注册",
        registerFailed: "注册失败",
        conflict: "用户名已经被占用！",
        other: "注册失败，请重试！",
      },
    },
    identity: {
      account: {
        basic: {
          profile: {
            title: "基本信息",
            id: "用户ID",
            username: "用户名",
            email: "电子邮箱",
            update: "更新",
            opName: "基本信息修改",
            failedDescription: "请重试。",
          },
          changePassword: {
            title: "修改密码",
            original: "原密码",
            newPassword: "新密码",
            update: "更新",
            opName: "密码修改",
            failedDescription: "请检查密码是否输入正确，再重试。",
          },
        },
        joinedDomains: {
          title: "加入的域",
          table: {
            id: "域ID",
            name: "域名字",
            role: "角色",
            actions: "操作",
            exit: "退出域",
            confirmExit: "确定要退出域 {} 吗？",
            okText: "确定",
            cancelText: "取消",
            opName: "退出域",
            error: {
              isPayAccount: "您是这个域的付款账号。请先将付款账号修改为其他账号，再删除。",
              notJoined: "您未加入这个域！",
            }
          },
        },
        joinDomain: {
          link: "申请加入新的域",
          title: "加入一个域",
          name: "域名",
          prompt: "请选择要加入的域",
          opName: "加入域",
        }
      },
      domains: {
        title: "域管理",
        id: "域ID",
        name: "域名字",
        active: {
          title: "状态",
          true: "可用",
          false: "停用",
        },
        payUser: "付款用户",
        admins: "管理员",
        resources: "分配的资源",
        actions: "操作",
        edit: "编辑",
        setResources: {
          link: "分配资源",
          title: "分配资源",
        },
        setAdmins: {
          link: "设置管理员",
          title: "设置管理员",
          payUserMustBeSelected: "付款用户必须是管理员。",
        },
        setPayUser: {
          link: "设置付款用户",
          title: "设置付款用户",
        },
        add: {
          button: "创建域",
          title: "创建域",
          name: "域名字",
          payUser: "付款账号，也是域的一个管理员",
          opName: "创建域",
          conflict: "已有同名域存在。请更换一个域。",
        },
      },
      projects: {
        title: "项目管理",
        create: "创建项目",
        table: {
          id: "项目ID",
          name: "项目名",
          active: {
            title: "状态",
            true: "可用",
            false: "停用",
          },
          payUser: "付款用户",
          users: "用户",
          actions: "操作",
          resources: "分配的资源",
          edit: "编辑",
          manageUsers: {
            link: "管理用户",
          },
        },
      },
    },
    resources: {
    },
    nav: {
      errorPage: {
        title: "无权限或不存在",
        description: "对不起，您所访问的页面不存在或者您没有权限访问",
        backToHome: "回到资源管理",
      },
      scopeIndicator: {
        projects: "项目",
        domains: "学校",
        system: "系统",
        admin: "管理员",
        opName: "切换",
        failDescription: "请退出登录重试",
        changing: "正在切换到{}...",
      },
      user: {
        selfCenter: "账号管理",
        logout: "登出",
      },
      sidenav: {
        resources: {
          root: "资源管理",
          dashboard: "仪表盘",
          instance: "实例",
          network: "网络",
          volume: "云硬盘",
        },
        expenses: {
          root: "费用管理",
        },
        identity: {
          root: "用户管理",
          selfcenter: "个人中心",
          account: {
            root: "个人账号管理",
            basic: "基本信息",
            domains: "加入的域",
            join: "加入新域",
          },
          domains: "域管理",
          projects: "项目管理",
          users: "用户管理",
        },
      },
    },
    header: {
      resources: "资源管理",
      expenses: "费用管理",
      identity: "用户管理"
    },
    footer: {
      description: "为高校设计的云平台",
      contact: {
        title: "联系方式",
        github: "GitHub - ddadaal",
        website: "个人网站 - ddadaal.me",
        linkedin: "LinkedIn - Chen Junda",
      },
      moreProducts: {
        title: "更多产品",
        chainstore: "ChainStore - 基于区块链的分布式存储解决方案",
        chainpaper: "ChainPaper - 基于区块链的论文社交平台",
        aplusquant: "A+Quant - 基于机器学习的大类资产管理系统",
        tagx00: "Tag x00 - 基于机器学习的众包标注平台",
        lightx00: "Light x00 - 灯具进销存管理系统",
      },
      copyright: {
        madeWithLove: "用 ❤ 制作",
      }
    },
  }
}

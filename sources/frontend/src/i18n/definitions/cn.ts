import zh_CN from "antd/es/locale/zh_CN";

export default {
  metadata: {
    id: "cn",
    langStrings: ["cn", "zh-CN", "zh"],
    detailedId: "zh-CN",
    dayjsLocale: "zh-cn",
    name: "简体中文",
    antdConfigProvider: {
      locale: zh_CN,
    },
  },
  definitions: {
    validateMessages: {
      email: "请输入有效的电子邮箱地址。",
      required: "这是必需字段。",
      number: "请输入数字。",
      integer: "请输入整数。",
    },
    components: {
      billings: {
        refresh: "刷新",
        current: "各个{}当前{}周期",
        history: "{} {} 历史{}周期",
        Allocated: "计费",
        Used: "使用",
        subjectType: {
          Domain: "域",
          Project: "项目",
          User: "用户",
          UserProjectAssignment: "项目用户",
        },
        table: {
          quota: "资源限额",
          amount: "应付",
          payer: "支付人",
          nextDue: "下一次结算时间",
          startTime: "开始时间",
          endTime: "结束时间",
          actions: "操作",
          history: "历史",
        },
        stats: {
          title: "{} {} 当前{}周期",
          history: "历史周期",
          quota: "资源限额",
          amount: "应支付额",
          nextDue: "下一次结算时间",
          payer: "支付人",
        }
      },
      flavor: {
        title: "配置",
        type: "类型",
        value: "值",
        id: "ID",
        name: "配置名",
        cpu: "CPU核心数",
        memory: "内存 (MB)",
        rootDisk: "根目录磁盘大小 (GB)"
      },
      resources: {
        modalTitle: "资源",
        resourceType: "资源类型",
        values: "资源量",
        loadingQuota: "加载可用资源量中……",
        used: "已经使用",
        available: "可以分配",
        total: "共",
        number: "请输入一个大于等于已用值，小于等于可用值的数字。",
        cpu: "CPU核心数",
        memory: "内存 (MB)",
        storage: "存储 (GB)",
        setResources: {
          link: "设置资源限额",
          title: "设置资源限额",
        },
      },
      transactions: {
        account: {
          time: "时间",
          amount: "交易额",
          reason: "交易原因",
        },
        org: {
          time: "时间",
          payer: "支付人",
          receiver: "接受者",
          amount: "交易额",
          reason: "交易原因",
        },
        type: {
          Charge: "充值",
          UserManagementFee: "用户管理费",
          DomainManagementFee: "域管理费",
          ProjectManagementFee: "项目管理费",
          DomainResources: "域资源费用周期结算",
          ProjectResources: "项目资源费用周期结算",
          DomainQuotaChange: "域更改资源",
          ProjectQuotaChange: "项目更改资源",
          SocialResourcesChange: "社会项目资源使用变化",
        },
      },
      users: {
        id: "ID",
        username: "用户名",
        name: "姓名",
        email: "电子邮箱",
        setAsPayUser: {
          link: "设置为付款用户",
          prompt: "确定把这个用户设定为付款用户吗？",
          opName: "设置付款用户",
          mustAdmin: "付款用户必须是管理员。",
        },
        active: {
          title: "状态",
          true: "可用",
          false: "停用",
        },
        quota: "资源配额",
        role: {
          title: "角色",
          Admin: "管理员",
          Member: "成员",
        },
        payUser: {
          title: "付款用户",
          yes: "是",
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
            name: "姓名",
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
              cannotExitCurrentDomain: "不能退出现在正处于的域中！请切换到其他域再退出这个域。",
              cannotExitSocialDomain: "不能退出社会域！社会域是所有用户都在的公共域。",
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
        quota: "资源配额",
        actions: "操作",
        edit: "编辑",
        setAdmins: {
          link: "设置管理员",
          title: "设置管理员",
          payUserMustBeSelected: "付款用户必须是管理员。",
        },
        setPayUser: {
          link: "设置付款用户",
          title: "设置付款用户",
        },
        delete: {
          link: "删除",
          opName: "删除域",
          confirmPrompt: "确定要删除域 {} 吗？",
          inactive: "域正处于不可用状态，请先充值将项目启用，再进行删除。",
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
        create: {
          button: "创建项目",
          opName: "创建项目",
          name: "项目名",
          payUser: "付款账号，也是域的一个管理员",
          conflict: "已有同名项目存在。请更换一个名字。",
        },
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
          quota: "资源配额",
          edit: "编辑",
          manageUsers: {
            link: "管理用户",
            close: "关闭",
            closeAndRefresh: "关闭并刷新",
          },
          delete: {
            link: "删除",
            opName: "删除项目",
            confirmPrompt: "确定要删除项目 {} 吗？",
            inactive: "项目正处于不可用状态，请先充值将项目启用，再进行删除。",
          },
        },
      },
      users: {
        title: "用户管理",
        refresh: "刷新",
        id: "ID",
        username: "用户名",
        name: "姓名",
        email: "电子邮箱",
        active: {
          title: "状态",
          true: "可用",
          false: "停用",
        },
        actions: "操作",
        remove: {
          link: "移除用户",
          confirmPrompt: "确定要移除用户 {} 吗？",
          opName: "移除用户",
        }
      },
    },
    resources: {
      notAvailable: {
        title: "资源管理不可用",
        NotProjectScope: {
          title: "域范围不可以使用云资源",
          subTitle: "请切换到项目范围以使用项目的云资源。",
        },
        UserNotActive: {
          title: "您的用户目前余额小于0",
          subTitle: "先给您的用户充值再继续使用云资源。"
        },
        ScopeNotActive: {
          title: "您的范围目前由于欠费不可用",
          subTitle: "请报告给您目前范围的管理员。"
        },
        toExpenses: "去资费管理",

      },
      instance: {
        id: "ID",
        name: "名字",
        ip: "IP地址",
        flavor: "配置",
        imageName: "镜像名字",
        status: {
          label: "状态",
          SHUTOFF: "关机",
          BUILD: "构建中",
          ERROR: "出错",
          ACTIVE: "开机",
          REBOOT: "软重启",
          HARD_REBOOT: "硬重启",
          STOPPED: "停止",
        },
        taskState: {
          label: "任务状态",

        },
        powerState: {
          label: "电源状态",
        },
        vmState: {
          label: "虚拟机状态",
        },
        createTime: "创建时间",
        startTime: "总开机时间",
        title: "实例管理",
        refresh: "刷新",
        actions: {
          title: "操作",
          start: "开启实例",
          stop: "关闭虚拟机",
          restart: "软重启虚拟机",
          hardRestart: "硬重启虚拟机",
          delete: "删除虚拟机",
        },
        add: {
          button: "创建新实例",
          name: "名字",
          flavor: "配置",
          available: "可用",
          flavorLimit: "您所选择的配置必须小于或者等于可用配置。",
          volume: "磁盘大小 (GB)",
          volumeLimit: "要创建的磁盘大小必须小于或者等于可用磁盘空间。",
          image: "启动镜像",
          opName: "创建实例",
        }
      },
      volume: {
        title: "查看云硬盘",
        refresh: "刷新",
        id: "ID",
        size: "大小",
        createTime: "创建时间",
        attachedToInstance: "挂载到实例",
        attachedToDevice: "实例上的设备路径"
      },
      dashboard: {
        title: "仪表盘",
        instanceList: {
          title: "实例列表",
          detail: "查看详细",
        }
      },
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
        social: "公开",
        system: "系统",
        admin: "管理员",
        opName: "切换",
        failDescription: "请退出登录重试",
        changing: "正在切换到{}...",
        reloading: "重新加载中...",
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
          overview: "总览",
          accountTransactions: "账户交易记录",
          systemTransactions: "系统交易记录",
          domainTransactions: "域交易记录",
          projectTransactions: "项目交易记录",
          billings: {
            detail: "详情",
            allocated: "分配",
            used: "使用",
            domains: "系统中域计费",
            domain: "本域计费",
            projects: "域中项目计费",
            project: "本项目计费",
            users: "用户计费",
            user: "本用户计费",
          },
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
    expenses: {
      root: "费用管理",
      overview: {
        root: "总览",
        balance: "余额",
        toAccountTransaction: "查看账户交易记录",
        charge: {
          button: "充值",
          opName: "充值",
          amount: "数量",
        },
        system: {
          title: "最近5个系统交易记录",
          link: "查看更多系统交易记录",
        },
        projectAdmin: {
          link: "查看当前项目计费周期信息",
        },
        domainAdmin: {
          link: "查看当前域计费周期信息"
        }
      },
      accountTransactions: {
        title: "账户交易记录",
        refresh: "刷新",
      },
      systemTransactions: {
        title: "系统交易记录",
        refresh: "刷新",
      },
      domainTransactions: {
        title: "域交易记录",
        refresh: "刷新",
      },
      projectTransactions: {
        title: "项目交易记录",
        refresh: "刷新",
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

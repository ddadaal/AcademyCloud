import zh_CN from "antd/es/locale/zh_CN";

export default {
  metadata: {
    id: "cn",
    langStrings: ["cn", "zh-CN", "zh"],
    detailedId: "zh-CN",
    name: "简体中文",
    antdConfigProvider: {
      locale: zh_CN,
      form: {
        validateMessages: {
          required: "这是必需字段。",
          types: {
            email: "请输入有效的电子邮箱地址。",
          }
        }
      }
    },
  },
  definitions: {
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
      sidebar: {
        account: {
          root: "个人账号管理",
          basic: "基本信息",
          domains: "域认证",
        },
        domains: "域管理",
        projects: "项目管理",
        users: "用户管理",
      },
      account: {
        basic: {
          profile: {
            title: "基本信息",
            id: "用户ID",
            username: "用户名",
            email: "电子邮箱",
            update: "更新",
            success: "基本信息修改成功！",
            failed: "基本信息修改失败！",
            failedDescription: "请重试。",
          },
          changePassword: {
            title: "修改密码",
            original: "原密码",
            newPassword: "新密码",
            update: "更新",
            success: "密码修改成功！",
            failed: "密码修改失败！",
            failedDescription: "请检查密码是否输入正确，再重试。",
          },
        }
      }
    },
    resources: {
      sidebar: {
        dashboard: "控制台",
        instance: "实例",
        network: "网络",
        volume: "云硬盘",
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
        system: "系统",
        admin: "管理员",
        success: "切换成功！",
        fail: "切换失败，请退出登录重试",
        changing: "正在切换到{}...",
      },
      user: {
        selfCenter: "个人中心",
        logout: "登出",
      },
      sidenav: {
        resources: {
          root: "资源管理",
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
        },
      },
      navPoints: {
        homepage: "登录",
        resources: {
          root: "资源管理",
          dashboard: "控制台",
          instance: "实例",
          network: "网络",
          volume: "云硬盘",
        },
        identity: {
          root: "用户管理",
          account: {
            root: "个人账号管理",
            basic: "基本信息",
            domains: "域认证",
          },
          domains: "域管理",
          projects: "项目管理",
          users: "用户管理",
        },
      }
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
};

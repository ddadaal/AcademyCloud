export default {
  metadata: {
    id: "cn",
    langStrings: ["cn", "zh-CN", "zh"],
    detailedId: "zh-CN",
    name: "简体中文",
  },
  definitions: {
    homepage: {
      title: "AcademyCloud",
      description: "为大学设计的云平台",
      loginForm: {
        school: "学校",
        schoolPrompt: "请输入学校",
        username: "用户名",
        usernamePrompt: "请输入用户名",
        password: "密码",
        passwordPrompt: "请输入密码",
        login: "登录",
        remember: "记住我",
        forget: "忘记密码",
        loginFailTitle: "登录失败",
        noScope:  "您未加入任何学校或者项目！请联系管理员。",
        other: "登录失败",
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
      user: {
        selfCenter: "个人中心",
        logout: "登出",
      },
      navPoints: {
        homepage: "登录",
        resources: {
          root: "资源管理",
          dashboard: "控制台",
          instance: "实例",
          network: "网络",
          volume: "云硬盘",
        }
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
        github:"GitHub - ddadaal",
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
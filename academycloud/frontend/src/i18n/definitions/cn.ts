export default {
  metadata: {
    id: "cn",
    gitalkLangId: "zh-CN",
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
      breadcrumbLeader: "位置",
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
  }
};

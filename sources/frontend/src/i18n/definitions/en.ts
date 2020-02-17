export default {
  metadata: {
    id: "en",
    langStrings: ["en", "en-US"],
    detailedId: "en-US",
    name: "English",
  },
  definitions: {
    homepage: {
      title: "AcademyCloud",
      description: "Cloud for Academy",
      pageIndicator: {
        login: "Login",
        register: "Register",
      },
      loginForm: {
        username: "Username",
        usernamePrompt: "Please input username.",
        password: "Password",
        passwordPrompt: "Please input password.",
        login: "Login",
        remember: "Remember me",
        forget: "Forget password",
        loginFailTitle: "Login failed.",
        noScope: "You are not part of any schools or projects. Please contact administrator.",
        other: "Login Failed",
      },
      registerForm: {
        username: "Username",
        usernamePrompt: "Please input username.",
        password: "Password",
        passwordPrompt: "Please input password.",
        email: "Email",
        emailPrompt: "Please input email.",
        register: "Register",
        registerFailed: "Register failed",
        conflict: "Username has been token.",
        other: "Register failed. Please retry.",
      },
    },
    resources: {
      sidebar: {
        dashboard: "Dashboard",
        instance: "Instances",
        network: "Network",
        volume: "Volumes",
      },
    },
    nav: {
      user: {
        selfCenter: "Self Center",
        logout: "Logout",
      },
      navPoints: {
        homepage: "Login",
        resources: {
          root: "Resources",
          dashboard: "Dashboard",
          instance: "Instances",
          network: "Network",
          volume: "Volumes",
        }
      },
      sidenav: {
        resources: {
          root: "Resources",
          instance: "Instances",
          network: "Network",
          volume: "Volume",
        },
        expenses: {
          root: "Expenses",
        },
        identity: {
          root: "Identity",
          selfcenter: "Self Center",
        },
      },
    },

    header: {
      resources: "Resources",
      expenses: "Expenses",
      identity: "Identity"
    },
    footer: {
      description: "Cloud for Academy",
      contact: {
        title: "Contact",
        github: "GitHub - ddadaal",
        website: "Personal Website - ddadaal.me",
        linkedin: "LinkedIn - Chen Junda"
      },
      moreProducts: {
        title: "More Products",
        chainpaper: "ChainPaper - Paper Social Platform powered by Blockchain",
        chainstore: "ChainStore - Distributed Storage System based on Blockchain",
        aplusquant: "A+Quant - An Asset Allocation System based on ML",
        tagx00: "Tag x00 - Online Tagging Platform powered by ML",
        lightx00: "Light x00 - Light Product Purchasing-Selling-Stocking System",
      },
      copyright: {
        madeWithLove: "Made with ❤",
      },
    },
  },
};

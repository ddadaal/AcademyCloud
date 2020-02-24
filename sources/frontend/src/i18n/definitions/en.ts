import en_US from "antd/es/locale/en_US";

export default {
  metadata: {
    id: "en",
    langStrings: ["en", "en-US"],
    detailedId: "en-US",
    name: "English",
    antdConfigProvider: {
      locale: en_US,
      form: {
        validateMessages: {
          required: "Please input this field.",
          types: {
            email: "Please input a valid email.",
          }
        }
      }
    },
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
        title: "Login",
        username: "Username",
        password: "Password",
        login: "Login",
        remember: "Remember me",
        forget: "Forget password",
        loginFailTitle: "Login failed.",
        noScope: "You are not part of any schools or projects. Please contact administrator.",
        other: "Login Failed",
      },
      registerForm: {
        title: "Register",
        username: "Username",
        password: "Password",
        email: "Email",
        register: "Register",
        registerFailed: "Register failed",
        conflict: "Username has been token.",
        other: "Register failed. Please retry.",
      },
    },
    identity: {
      account: {
        basic: {
          profile: {
            title: "Basic Information",
            id: "User ID",
            username: "Username",
            email: "Email",
            update: "Update",
            success: "Basic information updated successfully!",
            failed: "Basic information update failed.",
            failedDescription: "Please retry.",
          },
          changePassword: {
            title: "Change Password",
            original: "Original password",
            newPassword: "New password",
            update: "Update",
            success: "Password updated successfully!",
            failed: "Password update failed.",
            failedDescription: "Please check the original password and retry.",
          },
        },
        joinedDomains: {
          title: "Joined Domains",
          table: {
            id: "Domain Id",
            name: "Domain Name",
            role: "Role",
            actions: "Actions",
            exit: "Exit Domain",
            confirmExit: "Confirm to exit domain {} ?",
            okText: "Confirm",
            cancelText: "Cancel",
            success: "Exit Successful!",
            error: {
              title: "Exit Failed!",
              isPayAccount: "You are the pay account for this domain. Please set the pay account of the domain to another account, and then exit.",
              notJoined: "You have not joined this domain!",
            }
          },
          join: "Join A New Domain",
        },
        joinDomain: {
          title: "Join A Domain",

        }
      },
    },
    resources: {
    },
    nav: {
      errorPage: {
        title: "Not Exists Or Not Authorized",
        description: "Sorry, the page you visited does not exist, or you are not authorized to access it.",
        backToHome: "Back to resource management",
      },
      scopeIndicator: {
        projects: "Projects",
        domains: "Domains",
        system: "System",
        admin: "Admin",
        success: "Switch successful.",
        fail: "Switch failed. Please retry.",
        changing: "Switching to {}...",
      },
      user: {
        selfCenter: "Manage Account",
        logout: "Logout",
      },
      sidenav: {
        resources: {
          root: "Resources",
          dashboard: "Dashboard",
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
          account: {
            root: "Personal Account",
            basic: "Basic Information",
            domains: "Joined Domains",
            join: "Join New Domain",
          },
          domains: "Domains",
          projects: "Projects",
          users: "Users",
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

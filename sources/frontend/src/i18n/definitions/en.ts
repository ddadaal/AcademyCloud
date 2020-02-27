import en_US from "antd/es/locale/en_US";

export default {
  metadata: {
    id: "en",
    langStrings: ["en", "en-US"],
    detailedId: "en-US",
    name: "English",
    antdConfigProvider: {
      locale: en_US,
    },
  },
  definitions: {
    validateMessages: {
      required: "Please input this field.",
      email: "Please input a valid email.",
    },
    components: {
      resources: {
        resourceType: "Resource Type",
        values: "Resource Quantity",
        cpu: "CPU Cores",
        memory: "Memory (MB)",
        storage: "Storage (GB)",
      },
      users: {
        id: "ID",
        username: "Username",
        name: "Name",
        email: "Email",
        active: {
          title: "Status",
          true: "Active",
          false: "Inactive",
        },
        role: {
          title: "Role",
          admin: "Admin",
          member: "Member",
        },
      },
      operationResult: {
        success: "{} success!",
        fail: "{} failed.",
        retry: "Please retry",
      },
    },
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
            opName: "Basic information update",
            failedDescription: "Please retry.",
          },
          changePassword: {
            title: "Change Password",
            original: "Original password",
            newPassword: "New password",
            update: "Update",
            opName: "Password update",
            failedDescription: "Please check the original password and retry.",
          },
        },
        joinedDomains: {
          title: "Joined Domains",
          table: {
            id: "Id",
            name: "Name",
            role: "Role",
            actions: "Actions",
            exit: "Exit Domain",
            confirmExit: "Confirm to exit domain {} ?",
            okText: "Confirm",
            cancelText: "Cancel",
            opName: "Exit domain",
            error: {
              isPayAccount: "You are the pay account for this domain. Please set the pay account of the domain to another account, and then exit.",
              notJoined: "You have not joined this domain!",
            }
          },
        },
        joinDomain: {
          link: "Join A New Domain",
          title: "Join A Domain",
          name: "Domain Name",
          prompt: "Select the domain you'd like to join",
          opName: "Join a domain",
        }
      },
      domains: {
        title: "Domains Management",
        id: "ID",
        name: "Name",
        active: {
          title: "Status",
          true: "Active",
          false: "Inactive",
        },
        payUser: "Pay User",
        admins: "Admins",
        resources: "Reources",
        actions: "Actions",
        edit: "Edit",
        setResources: {
          link: "Set Resources",
          title: "Set Resources",
        },
        setAdmins: {
          link: "Set Admins",
          title: "Set Admins",
          payUserMustBeSelected: "Pay User must be an admin.",
        },
        setPayUser: {
          link: "Set Pay User",
          title: "Set Pay User",
        },
        add: {
          button: "Create a New Domain",
          title: "Create a New Domain",
          name: "Name",
          payUser: "Pay User, also a domain admin",
          opName: "Create Domain",
          conflict: "The domain with the same exists. Please change.",
        }
      },
      projects: {
        title: "Projects",
        create: "Create New Project",
        table: {
          id: "ID",
          name: "Name",
          active: {
            title: "Status",
            true: "Active",
            false: "Inactive",
          },
          payUser: "Pay User",
          users: "Users",
          actions: "Action",
          resources: "Reources",
        },
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
        opName: "Switch",
        failDescription: "Please re-login.",
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

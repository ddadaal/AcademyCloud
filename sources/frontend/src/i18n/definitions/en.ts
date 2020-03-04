import en_US from "antd/es/locale/en_US";

export default {
  metadata: {
    id: "en",
    langStrings: ["en", "en-US"],
    detailedId: "en-US",
    dayjsLocale: "en",
    name: "English",
    antdConfigProvider: {
      locale: en_US,
    },
  },
  definitions: {
    validateMessages: {
      required: "Please input this field.",
      email: "Please input a valid email.",
      number: "Please input a valid number.",
      integer: "Please input a valid integer.",
    },
    components: {
      resources: {
        resourceType: "Resource Type",
        values: "Resource Quantity",
        cpu: "CPU Cores",
        memory: "Memory (MB)",
        storage: "Storage (GB)",
        setResources: {
          link: "Set Resources",
          title: "Set Resources",
        },
      },
      transactions: {
        account: {
          time: "Time",
          amount: "Amount",
          type: "Type",
          info: "Info",
        },
        org: {
          time: "Time",
          payer: "Payer",
          amount: "Amount",
          receiver: "Receiver",
          type: "Type",
          info: "Info",
        },
        type: {
          Charge: "Charge",
          UserManagementFee: "User Management Fee",
          DomainManagementFee: "Domain Management Fee",
          ProjectManagementFee: "Project Management Fee",
          DomainResources: "Domain Resources Fee",
          ProjectResources: "Project Resources Fee",
        },
      },
      users: {
        id: "ID",
        username: "Username",
        name: "Name",
        email: "Email",
        setAsPayUser: {
          link: "Set As Pay User",
          prompt: "Confirm to set this user as the pay user?",
          opName: "Set pay user",
          mustAdmin: "Pay user must be an admin.",
        },
        active: {
          title: "Status",
          true: "Active",
          false: "Inactive",
        },
        resources: "Resources",
        role: {
          title: "Role",
          admin: "Admin",
          member: "Member",
        },
        payUser: {
          title: "Pay User",
          yes: "Yes",
        },
        addUser: {
          button: "Add user",
          selectUser: "Select user (users already in are not shown)",
          selectRole: "Select role",
        },
        actions: "Actions",
        remove: {
          link: "Remove",
          opName: "Remove",
          prompt: "Sure to remove?",
          errors: {
            payUser: "Can't remove the pay user. Please change the pay user and try again.",
            onlyAdmin: "Can't remove the only admin. Please set more admins and try again.",
          },
        },
        changeRole: {
          opName: "Change role",
          errors: {
            payUser: "Can't change the role of pay user. Please change the pay user and try again.",
            onlyAdmin: "Can't change the role of the only admin. Please set more admins and try again.",
          },
        },
      },
      operationResult: {
        inProgress: "{} in progress...",
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
              cannotExitCurrentDomain: "Cannot exit current domain! Please switch to another domain before exiting current domain.",
              cannotExitSocialDomain: "Cannot exit social domain! All users must be in the social domain.",
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
        setAdmins: {
          link: "Set Admins",
          title: "Set Admins",
          payUserMustBeSelected: "Pay User must be an admin.",
        },
        setPayUser: {
          link: "Set Pay User",
          title: "Set Pay User",
        },
        delete: {
          link: "Delete",
          opName: "Delete domain",
          confirmPrompt: "Confirm to delete domain {}?",
          inactive: "Domain is inactive. Please active the domain by paying the debt, and then try again.",
        },
        add: {
          button: "Create a New Domain",
          title: "Create a New Domain",
          name: "Name",
          payUser: "Pay User, also a domain admin",
          opName: "Create Domain",
          conflict: "The domain with the same name already exists. Please choose another name.",
        }
      },
      projects: {
        title: "Projects",
        create: {
          button: "Create New Project",
          opName: "Create project",
          name: "Name",
          payUser: "Pay User, also a domain admin",
          conflict: "A project with the same name already exists in the domain. Please choose another name",
        },
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
          resources: "Reources",
          actions: "Action",
          edit: "Edit",
          manageUsers: {
            link: "Manage users",
            close: "Close",
            closeAndRefresh: "Close and refresh",
          },
          delete: {
            link: "Delete",
            opName: "Delete project",
            confirmPrompt: "Confirm to delete project {}?",
            inactive: "Project is inactive. Please active the project by paying the debt, and then try again.",
          },
        },
      },
      users: {
        title: "User Management",
        refresh: "Refresh",
        id: "ID",
        username: "Username",
        name: "Name",
        email: "Email",
        active: {
          title: "Status",
          true: "Active",
          false: "Inactive",
        },
        actions: "Actions",
        remove: {
          link: "Remove user",
          confirmPrompt: "Confirm to remove user {}?",
          opName: "Remove user",
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
        social: "Public",
        system: "System",
        admin: "Admin",
        opName: "Switch",
        failDescription: "Please re-login.",
        changing: "Switching to {}...",
        reloading: "Reloading...",
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
          overview: "Overview",
          accountTransactions: "Account Transactions",
          systemTransactions: "System Transactions",
          domainTransactions: "Domain Transactions",
          projectTransactions: "Project Transactions",
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
    expenses: {
      root: "Expenses",
      overview: {
        root: "Overview",
        balance: "Balance",
        toAccountTransaction: "To Account Transactions",
        charge: {
          button: "Charge",
          opName: "Charge",
          amount: "Amount",
        }
      },
      accountTransactions: {
        title: "Account Transactions",
        refresh: "Refresh",
      },
      systemTransactions: {
        title: "System Transactions",
        refresh: "Refresh",
      },
      domainTransactions: {
        title: "Domain Transactions",
        refresh: "Refresh",
      },
      projectTransactions: {
        title: "Project Transactions",
        refresh: "Refresh",
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

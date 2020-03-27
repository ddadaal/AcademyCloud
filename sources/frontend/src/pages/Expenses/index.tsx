import React from "react";
import { lang } from "src/i18n";
import { DashboardOutlined, TransactionOutlined, UserOutlined, ProjectOutlined, TeamOutlined, BankOutlined, PlayCircleOutlined, DatabaseOutlined } from "@ant-design/icons";
import { Scope, isSystemScope, isSocialScope, isDomainAdmin, isProjectAdmin, isProjectScope } from "src/models/Scope";
import { indexRoutes, IndexRoute } from "src/pages/common/indexRoutes";

const root = lang.nav.sidenav.expenses;

const AllocatedIcon = DatabaseOutlined;
const UsedIcon = PlayCircleOutlined;

const routes = [
  {
    path: "overview",
    textId: root.overview,
    Icon: DashboardOutlined,
    checkScope: () => true,
    Component: React.lazy(() => import("./Overview")),
  }, {
    path: "transactions/account",
    textId: root.accountTransactions,
    Icon: TransactionOutlined,
    checkScope: () => true,
    Component: React.lazy(() => import("./Transactions/Account"))
  },
  {
    path: "billings/user",
    textId: root.billings.user,
    Icon: UserOutlined,
    checkScope: (scope: Scope) => isProjectScope(scope),
    children: [
      {
        path: "allocated",
        textId: root.billings.allocated,
        Icon: AllocatedIcon,
        checkScope: (scope: Scope) => isProjectScope(scope),
        Component: React.lazy(() => import("./Billing/User/Allocated")),
      },
      {
        path: "used",
        textId: root.billings.used,
        Icon: UsedIcon,
        checkScope: (scope: Scope) => isProjectScope(scope),
        Component: React.lazy(() => import("./Billing/User/Used")),
      }
    ],
  },
  {
    path: "transactions/system",
    textId: root.systemTransactions,
    Icon: TransactionOutlined,
    checkScope: (scope: Scope) => isSystemScope(scope),
    Component: React.lazy(() => import("./Transactions/System"))
  },
  {
    path: "transactions/domain",
    textId: root.domainTransactions,
    Icon: TransactionOutlined,
    checkScope: (scope: Scope) => isDomainAdmin(scope),
    Component: React.lazy(() => import("./Transactions/Domain"))
  },
  {
    path: "transactions/project",
    textId: root.projectTransactions,
    Icon: TransactionOutlined,
    checkScope: (scope: Scope) => isProjectAdmin(scope),
    Component: React.lazy(() => import("./Transactions/Project"))
  },
  {

    path: "billings/domains",
    textId: root.billings.domains,
    Icon: BankOutlined,
    checkScope: (scope: Scope) => isSystemScope(scope),
    children: [
      {
        path: "allocated",
        textId: root.billings.allocated,
        Icon: AllocatedIcon,
        checkScope: (scope: Scope) => isSystemScope(scope),
        Component: React.lazy(() => import("./Billing/Domains/Allocated")),
      },
      {
        path: "used",
        textId: root.billings.used,
        Icon: UsedIcon,
        checkScope: (scope: Scope) => isSystemScope(scope),
        Component: React.lazy(() => import("./Billing/Domains/Used")),
      }
    ],

  }, {
    path: "billings/domain",
    textId: root.billings.domain,
    Icon: BankOutlined,
    checkScope: (scope: Scope) => isDomainAdmin(scope),
    children: [
      {
        path: "allocated",
        textId: root.billings.allocated,
        Icon: AllocatedIcon,
        checkScope: (scope: Scope) => isDomainAdmin(scope),
        Component: React.lazy(() => import("./Billing/Domain/Allocated")),
      },
      {
        path: "used",
        textId: root.billings.used,
        Icon: UsedIcon,
        checkScope: (scope: Scope) => isDomainAdmin(scope),
        Component: React.lazy(() => import("./Billing/Domain/Used")),
      }
    ],
  }, {
    path: "billings/projects",
    textId: root.billings.projects,
    Icon: ProjectOutlined,
    checkScope: (scope: Scope) => isDomainAdmin(scope),
    children: [
      {
        path: "allocated",
        textId: root.billings.allocated,
        Icon: AllocatedIcon,
        checkScope: (scope: Scope) => isDomainAdmin(scope),
        Component: React.lazy(() => import("./Billing/Projects/Allocated")),
      },
      {
        path: "used",
        textId: root.billings.used,
        Icon: UsedIcon,
        checkScope: (scope: Scope) => isDomainAdmin(scope),
        Component: React.lazy(() => import("./Billing/Projects/Used")),
      }
    ],
  }, {
    path: "billings/project",
    textId: root.billings.project,
    Icon: ProjectOutlined,
    checkScope: (scope: Scope) =>  isProjectAdmin(scope),
    children: [
      {
        path: "allocated",
        textId: root.billings.allocated,
        Icon: AllocatedIcon,
        checkScope: (scope: Scope) => !isSocialScope(scope) && isProjectAdmin(scope),
        Component: React.lazy(() => import("./Billing/Project/Allocated")),
      },
      {
        path: "used",
        textId: root.billings.used,
        Icon: UsedIcon,
        checkScope: (scope: Scope) => isProjectAdmin(scope),
        Component: React.lazy(() => import("./Billing/Project/Used")),
      }
    ],
  }, {
    path: "billings/users",
    textId: root.billings.users,
    Icon: TeamOutlined,
    checkScope: (scope: Scope) => !isSocialScope(scope) && isProjectAdmin(scope),
    children: [
      {
        path: "allocated",
        textId: root.billings.allocated,
        Icon: AllocatedIcon,
        checkScope: () => true,
        Component: React.lazy(() => import("./Billing/Users/Allocated")),
      },
      {
        path: "used",
        textId: root.billings.used,
        Icon: UsedIcon,
        checkScope: () => true,
        Component: React.lazy(() => import("./Billing/Users/Used")),
      }
    ],
  },
] as IndexRoute[];

const IdentityIndexPage = indexRoutes(routes, "/expenses/");

export default IdentityIndexPage;


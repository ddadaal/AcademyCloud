import React from "react";
import { lang } from "src/i18n";
import { DashboardOutlined, TransactionOutlined } from "@ant-design/icons";
import { Scope, isSystemScope, isSocialScope, isDomainAdmin, isProjectAdmin } from "src/models/Scope";
import { indexRoutes, IndexRoute } from "src/pages/common/indexRoutes";

const root = lang.nav.sidenav.expenses;

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
  }, {
    path: "transactions/system",
    textId: root.systemTransactions,
    Icon: TransactionOutlined,
    checkScope: (scope: Scope) => isSystemScope(scope),
    Component: React.lazy(() => import("./Transactions/System"))
  }, {
    path: "transactions/domain",
    textId: root.domainTransactions,
    Icon: TransactionOutlined,
    checkScope: (scope: Scope) => isDomainAdmin(scope),
    Component: React.lazy(() => import("./Transactions/Domain"))
  }, {
    path: "transactions/project",
    textId: root.projectTransactions,
    Icon: TransactionOutlined,
    checkScope: (scope: Scope) => isProjectAdmin(scope),
    Component: React.lazy(() => import("./Transactions/Project"))
  }, {
    path: "billings/domains",
    textId: root.billings.domains,
    Icon: TransactionOutlined,
    checkScope: (scope: Scope) => isSystemScope(scope),
    children: [
      {
        path: "allocated",
        textId: root.billings.allocated,
        Icon: TransactionOutlined,
        checkScope: (scope: Scope) => isSystemScope(scope),
        Component: React.lazy(() => import("./Billing/Domains/Allocated")),
      },
      {
        path: "used",
        textId: root.billings.used,
        Icon: TransactionOutlined,
        checkScope: (scope: Scope) => isSystemScope(scope),
        Component: React.lazy(() => import("./Billing/Domains/Used")),
      }
    ],

  }, {
    path: "billings/domain",
    textId: root.billings.domain,
    Icon: TransactionOutlined,
    checkScope: (scope: Scope) => isDomainAdmin(scope),
    children: [
      {
        path: "allocated",
        textId: root.billings.allocated,
        Icon: TransactionOutlined,
        checkScope: (scope: Scope) => isDomainAdmin(scope),
        Component: React.lazy(() => import("./Billing/Domain/Allocated")),
      },
      {
        path: "used",
        textId: root.billings.used,
        Icon: TransactionOutlined,
        checkScope: (scope: Scope) => isSystemScope(scope),
        Component: React.lazy(() => import("./Billing/Domains/Used")),
      }
    ],

  }, {
    path: "billings/projects",
    textId: root.billings.projects,
    Icon: TransactionOutlined,
    checkScope: (scope: Scope) => isDomainAdmin(scope),
    children: [
      {
        path: "allocated",
        textId: root.billings.allocated,
        Icon: TransactionOutlined,
        checkScope: (scope: Scope) => isDomainAdmin(scope),
        Component: React.lazy(() => import("./Billing/Projects/Allocated")),
      },
      {
        path: "used",
        textId: root.billings.used,
        Icon: TransactionOutlined,
        checkScope: (scope: Scope) => isDomainAdmin(scope),
        Component: React.lazy(() => import("./Billing/Projects/Used")),
      }
    ],
  }, {
    path: "billings/users",
    textId: root.billings.users,
    Icon: TransactionOutlined,
    checkScope: (scope: Scope) => !isSocialScope(scope) && isProjectAdmin(scope),
    children: [
      {
        path: "allocated",
        textId: root.billings.allocated,
        Icon: TransactionOutlined,
        checkScope: () => true,
        Component: React.lazy(() => import("./Billing/Users/Allocated")),
      },
      {
        path: "used",
        textId: root.billings.used,
        Icon: TransactionOutlined,
        checkScope: () => true,
        Component: React.lazy(() => import("./Billing/Users/Used")),
      }
    ],
  }
] as IndexRoute[];

const IdentityIndexPage = indexRoutes(routes, "/expenses/");

export default IdentityIndexPage;


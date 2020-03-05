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
  }
] as IndexRoute[];

const IdentityIndexPage = indexRoutes(routes, "/expenses/");

export default IdentityIndexPage;


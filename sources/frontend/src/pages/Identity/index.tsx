import React from "react";
import { lang } from "src/i18n";
import { UserOutlined, ProjectOutlined, TeamOutlined, BankOutlined, FormOutlined, PlusOutlined } from "@ant-design/icons";
import { Scope, isSystemScope } from "src/models/Scope";
import { indexRoutes, IndexRoute } from "src/pages/common/indexRoutes";

const root = lang.nav.sidenav.identity;

const routes = [
  {
    path: "account",
    textId: root.account.root,
    Icon: UserOutlined,
    children: [
      {
        path: "basic",
        textId: root.account.basic,
        Icon: FormOutlined,
        checkScope: () => true,
        Component: React.lazy(() => import("./Account/BasicInformation")),
      }, {
        path: "joinedDomains",
        textId: root.account.domains,
        Icon: BankOutlined,
        checkScope: (scope: Scope) => !isSystemScope(scope),
        Component: React.lazy(() => import("./Account/JoinedDomains")),
      }, {
        path: "joinDomain",
        textId: root.account.join,
        Icon: PlusOutlined,
        checkScope: (scope: Scope) => !isSystemScope(scope),
        Component: React.lazy(() => import("./Account/JoinDomain")),
      }
    ],
    checkScope: () => true,
    Component: React.lazy(() => import("./Account"))
  }, {
    path: "domains",
    textId: root.domains,
    Icon: BankOutlined,
    checkScope: (scope: Scope) => isSystemScope(scope),
    Component: React.lazy(() => import("./Domains"))
  }, {
    path: "projects",
    textId: root.projects,
    Icon: ProjectOutlined,
    checkScope: (scope: Scope) => !isSystemScope(scope),
    Component: React.lazy(() => import("./Projects"))
  }, {
    path: "users",
    textId: root.users,
    Icon: TeamOutlined,
    // domain member (user not in a project) can't enter users
    checkScope: (scope: Scope) => !(!scope.projectId && scope.role === "member"),
    Component: React.lazy(() => import("./Users"))
  },
] as IndexRoute[];

const IdentityIndexPage = indexRoutes(routes, "/identity/");

export default IdentityIndexPage;


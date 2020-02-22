import React, { useMemo } from "react";
import { NavItemProps } from 'src/layouts/nav/NavItemProps';
import { lang } from "src/i18n";
import { UserOutlined, ProjectOutlined, TeamOutlined, BankOutlined } from "@ant-design/icons";
import { RouteComponentProps, Router } from "@reach/router";
import { useSidenavs } from "src/layouts/nav/useSidenavs";
import { Scope, isSystemScope } from "src/models/Scope";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";

const root = lang.identity.sidebar;

const AsyncAccountPage = React.lazy(() => import("./Account"));
const AsyncDomainsPage = React.lazy(() => import("./Domains"));
const AsyncProjectsPage = React.lazy(() => import("./Projects"));
const AsyncUsersPage = React.lazy(() => import("./Users"));

interface PrivilegedNav extends NavItemProps {
  checkScope: (scope: Scope) => boolean;
  Component: ReturnType<typeof React.lazy>;
}


const sidenavs = [
  {
    path: "account",
    textId: root.account,
    Icon: UserOutlined,
    match: (path: string) => path === "/identity/account",
    checkScope: () => true,
    Component: AsyncAccountPage,
  }, {
    path: "domains",
    textId: root.domains,
    Icon: BankOutlined,
    match: (path: string) => path === "/identity/domains",
    checkScope: (scope: Scope) => isSystemScope(scope),
    Component: AsyncDomainsPage,
  }, {
    path: "projects",
    textId: root.projects,
    Icon: ProjectOutlined,
    match: (path: string) => path === "/identity/projects",
    checkScope: (scope: Scope) => !isSystemScope(scope),
    Component: AsyncProjectsPage,
  }, {
    path: "users",
    textId: root.users,
    Icon: TeamOutlined,
    match: (path: string) => path === "/identity/users",
    // domain member (user not in a project) can't enter users
    checkScope: (scope: Scope) => !(!scope.projectId && scope.role === "member"),
    Component: AsyncUsersPage,
  },
] as PrivilegedNav[];

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default function IdentityIndexPage(_: RouteComponentProps) {

  const userStore = useStore(UserStore);

  const { scope } = userStore.user!!;

  const filteredSidenavs = useMemo(() => (
    sidenavs.filter((x) => x.checkScope(scope))
  ), [scope]);

  console.log(filteredSidenavs);

  useSidenavs(filteredSidenavs, "/identity/");

  return (
    <Router>
      {sidenavs.map(({ Component, path }) => (
        <Component key={path} path={path} />
      ))}
    </Router>
  )
}


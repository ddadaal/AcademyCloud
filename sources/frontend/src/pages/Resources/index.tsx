import { RouteComponentProps, Router } from "@reach/router";
import React, { } from "react";
import { lang } from "src/i18n";
import { DashboardOutlined, DesktopOutlined } from "@ant-design/icons";
import { useSidenavs } from "src/layouts/nav/useSidenavs";

const AsyncDashboard = React.lazy(() => import("./Dashboard"));
const AsyncInstance = React.lazy(() => import("./Instance"));

const root = lang.resources.sidebar;

const sidebarRoutes = [
  {
    path: "/resources",
    textId: root.dashboard,
    Icon: DashboardOutlined,
    match: (path: string) => path === "/resources",
  },
  {
    path: "/resources/instances",
    textId: root.instance,
    Icon: DesktopOutlined,
    match: (path: string) => path === "/resources/instances",
  }
];

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default function ResourcesPage(_: RouteComponentProps) {

  useSidenavs(sidebarRoutes);

  return (
    <Router>
      <AsyncDashboard path="./" />
      <AsyncInstance path="instances/*" />
    </Router>
  )
};


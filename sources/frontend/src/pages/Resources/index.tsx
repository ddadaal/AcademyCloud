import React, { } from "react";
import { lang } from "src/i18n";
import { DashboardOutlined, DesktopOutlined } from "@ant-design/icons";
import { IndexRoute, indexRoutes } from "src/pages/common/indexRoutes";

const root = lang.resources.sidebar;

const routes = [
  {
    path: "dashboard",
    textId: root.dashboard,
    Icon: DashboardOutlined,
    match: (path: string) => path === "/resources/dashboard",
    checkScope: () => true,
    Component: React.lazy(() => import("./Dashboard")),
  }, {
    path: "instances",
    textId: root.instance,
    Icon: DesktopOutlined,
    match: (path: string) => path === "/resources/instances",
    checkScope: () => true,
    Component: React.lazy(() => import("./Instance")),
  }
] as IndexRoute[];

const ResourcesIndexPage = indexRoutes(routes, "/resources/")

export default ResourcesIndexPage;


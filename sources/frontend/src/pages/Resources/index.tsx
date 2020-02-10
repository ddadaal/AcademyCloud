import { RouteComponentProps, Router } from "@reach/router";
import React, { useEffect, Suspense } from "react";
import FunctionLayout from "src/layouts/FunctionLayout";
import RootLayout from "src/layouts/RootLayout";
import { useStore } from "simstate";
import { NavStore } from "src/layouts/nav/NavStore";
import { lang } from "src/i18n";
import { DashboardOutlined, DesktopOutlined } from "@ant-design/icons";
import PageLoading from "src/components/PageLoading";

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

export default function ResourcesPage(props: RouteComponentProps) {

  const navStore = useStore(NavStore);

  useEffect(() => {
    navStore.setSidenavs(sidebarRoutes);

    return () => { navStore.setSidenavs([]);}
  }, [navStore]);

  return (
    <Router>
      <AsyncDashboard path="./"/>
      <AsyncInstance path="instances/*"/>
    </Router>
  )
};


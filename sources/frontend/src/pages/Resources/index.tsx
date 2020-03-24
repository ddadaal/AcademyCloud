import React, {  useEffect } from "react";
import { lang } from "src/i18n";
import { DashboardOutlined, DesktopOutlined, HddOutlined } from "@ant-design/icons";
import { IndexRoute, indexRoutes } from "src/pages/common/indexRoutes";
import { RouteComponentProps } from "@reach/router";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { isResourcesDisabled } from "src/models/Scope";
import { NotAvailableReason, ResourcesNotAvailable } from "./ResourcesNotAvailable";

const root = lang.nav.sidenav.resources;

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
  }, {
    path: "volumes",
    textId: root.volume,
    Icon: HddOutlined,
    match: (path: string) => path === "/resources/volumes",
    checkScope: () => true,
    Component: React.lazy(() => import("./Volume")),
  }
] as IndexRoute[];

const ResourcesIndexRoutes = indexRoutes(routes, "/resources/")

const ResourcesIndexPage: React.FC<RouteComponentProps> = (props) => {
  const userStore = useStore(UserStore);

  useEffect(() => {
    userStore.updateAvailability();
  });

  const { scope, scopeActive, userActive } = userStore.user!;

  // judge whether resources available
  const reason = isResourcesDisabled(scope) ? NotAvailableReason.NotProjectScope :
    (!userActive) ? NotAvailableReason.UserNotActive :
      (!scopeActive) ? NotAvailableReason.ScopeNotActive :
        null;

  if (reason) {
    return (
      <ResourcesNotAvailable reason={reason} path="resources/*" />
    );
  }

  return (
    <ResourcesIndexRoutes {...props} />
  )
};

export default ResourcesIndexPage;


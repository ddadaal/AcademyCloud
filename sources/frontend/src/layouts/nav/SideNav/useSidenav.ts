import React, { useEffect } from "react";
import { lang } from "src/i18n";
import { DashboardOutlined, MoneyCollectOutlined, SmileOutlined, DesktopOutlined } from "@ant-design/icons"
import { NavItemProps } from "../NavItemProps";
import { useStore } from "simstate";
import { NavStore } from "../NavStore";

const root = lang.nav.sidenav;

const sidebarRoutes = [
  {
    path: "/resources",
    textId: root.resources.root,
    Icon: DashboardOutlined,
    match: (path: string) => path === "/resources",
    children: [
      {
        path: "/resources/instances",
        textId: root.resources.instance,
        Icon: DesktopOutlined,
        match: (path: string) => path === "/resources/instances",
      },
      {
        path: "/resources/network",
        textId: root.resources.network,
        Icon: DesktopOutlined,
        match: (path: string) => path === "/resources/network",
      }
    ]
  },
] as NavItemProps[];

export function useSidenav({ children }: React.Props<{}>) {

  const navStore = useStore(NavStore);

  useEffect(() => {
    navStore.setSidenavs(sidebarRoutes);
    return () => navStore.setSidenavs([]);
  }, [navStore]);

  return children;

}

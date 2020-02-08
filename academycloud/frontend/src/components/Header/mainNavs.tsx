import { NavItemProps } from "./NavItemProps";
import { DashboardTwoTone, MoneyCollectTwoTone, SmileTwoTone } from "@ant-design/icons"
import React from "react";
import { lang } from "src/i18n";

const root = lang.header;

export const mainNavs = [{
  path: "/dashboard",
  icon: DashboardTwoTone,
  textId: root.dashboard,
  match: (pathname: string) => pathname.startsWith("/dashboard"),
}, {
  path: "/expenses",
  icon: MoneyCollectTwoTone,
  textId: root.expenses,
  match: (pathname: string) => pathname.startsWith("/expenses"),
}, {
  path: "/identity",
  icon: SmileTwoTone,
  textId: root.identity,
  match: (pathname: string) => pathname.startsWith("/identity")
}
];

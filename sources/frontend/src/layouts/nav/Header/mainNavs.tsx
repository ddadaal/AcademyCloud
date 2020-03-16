import { DashboardOutlined, MoneyCollectOutlined, SmileOutlined } from "@ant-design/icons"
import { lang } from "src/i18n";
import { NavItemProps } from "src/layouts/nav/NavItemProps";
import { Scope, isResourcesDisabled } from "src/models/Scope";

const root = lang.header;

export const mainNavs = [{
  path: "/resources",
  Icon: DashboardOutlined,
  textId: root.resources,
  match: (pathname: string) => pathname.startsWith("/resources"),
}, {
  path: "/expenses",
  Icon: MoneyCollectOutlined,
  textId: root.expenses,
  match: (pathname: string) => pathname.startsWith("/expenses"),
}, {
  path: "/identity",
  Icon: SmileOutlined,
  textId: root.identity,
  match: (pathname: string) => pathname.startsWith("/identity")
}
] as NavItemProps[];

export function selectMainNavs(scope?: Scope): NavItemProps[] {
  // bad code, but works, no time to refactor this.
  if (isResourcesDisabled(scope)) {
    return mainNavs.filter(x => x.path !== "/resources");
  }
  return mainNavs;
}

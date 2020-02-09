import { DashboardOutlined, MoneyCollectOutlined, SmileOutlined } from "@ant-design/icons"
import { lang } from "src/i18n";
import { NavItemProps } from "src/layouts/nav/NavItemProps";

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

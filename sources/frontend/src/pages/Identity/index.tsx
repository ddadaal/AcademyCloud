import React from "react";
import { NavItemProps } from 'src/layouts/nav/NavItemProps';
import { lang } from "src/i18n";
import { UserOutlined } from "@ant-design/icons";

const root = lang.identity.sidebar;

const sidenavs = [
  {
    path: "/identity/account",
    textId: root.account,
    Icon: UserOutlined,
    match: (path: string) => path === "/identity/account",
  },
] as NavItemProps[];


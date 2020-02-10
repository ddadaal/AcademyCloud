import { UserStore } from "../../../stores/UserStore";
import React, { useCallback } from "react";
import { Dropdown, Avatar, Menu } from "antd";
import MediaQuery from "react-responsive";
import { useStore } from "simstate";
import { Link, navigate } from "@reach/router";
import { LocalizedString, lang } from "src/i18n";
import { layoutConstants } from "src/layouts/constants";
import { DownOutlined } from "@ant-design/icons";

const DEFAULT_AVATAR = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1540823688873&di=18f09cc1a41075294d1c12e67d178594&imgtype=0&src=http%3A%2F%2Fku.90sjimg.com%2Felement_origin_min_pic%2F16%2F06%2F05%2F155753d94b715a2.jpg";

const root = lang.nav.user;

export function UserIndicator() {

  const userStore = useStore(UserStore);

  const logout = useCallback(() => {
    userStore.logout();
    navigate("/");
  }, [userStore]);

  const dropdownMenu = <Menu>
    <Menu.Item key="self">
      <Link to={"/user"}><LocalizedString id={root.selfCenter}/></Link>
    </Menu.Item>
    <Menu.Divider/>
    <Menu.Item key="logout">
      <a onClick={logout}><LocalizedString id={root.logout}/></a>
    </Menu.Item>
  </Menu>;

  return (
    <Dropdown overlay={dropdownMenu}>
      <a className="ant-dropdown-link">
        <Avatar size="default" src={DEFAULT_AVATAR}/>
        <MediaQuery minWidth={layoutConstants.menuBreakpoint}>
          <span style={{marginLeft: "8px"}}>
            {userStore.user?.username ?? "undefined"}
            <DownOutlined />
          </span>
        </MediaQuery>
      </a>
    </Dropdown>
  );
}

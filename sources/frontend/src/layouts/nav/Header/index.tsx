import React, { useCallback } from "react";
import { Dropdown, Layout, Menu, Popover } from "antd";
import { BarsOutlined } from "@ant-design/icons";
import { mainNavs } from "./mainNavs";
import MediaQuery from "react-responsive";
import { layoutConstants } from "../../constants";
import styled from "styled-components";
import { Location, navigate, Link } from "@reach/router";
import {ReactComponent as LogoSvg} from "src/assets/logo-horizontal.svg";
import { LocalizedString } from "src/i18n";
import { useStore } from "simstate";
import { NavStore } from "src/layouts/nav/NavStore";
import HeaderIcon from "./HeaderIcon";
import { MenuFoldOutlined, MenuUnfoldOutlined }  from "@ant-design/icons";
import { useLocation } from "src/utils/useLocation";
import { LanguageSelector } from "./LanguageSelector";
import { UserIndicator } from "./UserIndicator";
import { UserStore } from "src/stores/UserStore";
const { Header: AntdHeader } = Layout;

interface Props {

}

export const MarginedDiv = styled.div<{ margin?: number}>`

  display: flex;
  align-items: center;
  & > * {
    margin-right: ${(props) => props.margin ?? 4}px;
    margin-left: ${(props) => props.margin ?? 4}px;
    //display: inline-block;
  }

  &:first-child {
    margin-left: 0px;
  }

  &:last-child {
  margin-right: 0px;
  }
`;

export const dropdownMenuStyle: React.CSSProperties = {
  width: "256px",
  textAlign: "center",
};

export const horizontalMenuStyle: React.CSSProperties = {
  lineHeight: `${layoutConstants.headerHeight}px`,
};


export const Head = styled(AntdHeader)`
  && {
    display: flex;
    align-items: center;
    justify-content: space-between;
    height: ${layoutConstants.headerHeight}px;
    padding: 4px;
    /* background-color: white; */

  }
`;

export function HeaderNavMenu(props: {
  vertical: boolean; selectedKeys: string[]; to(path: string): void;
}) {
  const { vertical, selectedKeys, to } = props;
  return (
    <Menu theme={"dark"}
      mode={vertical ? "vertical" : "horizontal"}
      selectedKeys={selectedKeys}
      style={vertical ? dropdownMenuStyle : horizontalMenuStyle}>
      {mainNavs.map((x) =>
        <Menu.Item
          key={x.path}
          onClick={() => to(x.path)}>
          {React.createElement(x.Icon)}
          <LocalizedString id={x.textId} />
        </Menu.Item>,
      )}
    </Menu>
  );
}

const StyledLogo = styled(LogoSvg)`

  height: 40px;
  width: auto;

  &:hover {
    cursor: pointer;
  }

`;

const CompactMenuItem = styled(Menu.Item)`
    padding: 0 8px;
`;

const FixedMenu: React.FC<{}> = () => (
  <Menu theme="dark" mode="horizontal" style={horizontalMenuStyle} selectable={false}>
    <CompactMenuItem>
      <UserIndicator />
    </CompactMenuItem>
    <CompactMenuItem>
      <LanguageSelector />
    </CompactMenuItem>
  </Menu>
)

const Logo = () => {
  const userStore = useStore(UserStore);

  const jump = useCallback(() => {
    if (userStore.loggedIn) {
      navigate("/resources");
    } else {
      navigate("/");
    }
  }, [userStore.loggedIn]);

  return (
    <StyledLogo onClick={jump}/>
  );
};

export const Header: React.FC = () => {
  const navStore = useStore(NavStore);

  const collapse = useCallback(() => {
    navStore.setSidebarCollapsed(!navStore.sidebarCollapsed);
  }, [navStore]);


  const selectedKeys = mainNavs.filter((x) => x.match(navStore.location.pathname)).map((x) => x.path);


  return (
    <Head>
      <MarginedDiv>
        {navStore.hasSider
          ? <a onClick={collapse}>
            <HeaderIcon
              Icon={navStore.sidebarCollapsed ? MenuUnfoldOutlined : MenuFoldOutlined}
            />
          </a>
          : null}
        <Logo />
      </MarginedDiv>
      <MarginedDiv margin={0}>
        {/* {fixedMenu} */}
        <FixedMenu/>
        {/* <MediaQuery minWidth={layoutConstants.menuBreakpoint}>
          {(matches) => matches
            ? <>
              <HeaderNavMenu vertical={false} selectedKeys={selectedKeys} to={navigate} />
              <FixedMenu />
            </>
            : <>
              <Dropdown overlay={
                <HeaderNavMenu
                  vertical={true}
                  selectedKeys={selectedKeys}
                  to={navigate}
                />} trigger={["click"]}>
                <HeaderIcon Icon={BarsOutlined} />
              </Dropdown>
            </>
          }
        </MediaQuery> */}
      </MarginedDiv>
    </Head>
  );
}


import React, { useCallback } from "react";
import { Dropdown, Layout, Menu } from "antd";
import { BarsOutlined } from "@ant-design/icons";
import { mainNavs } from "./mainNavs";
import MediaQuery from "react-responsive";
import { layoutConstants } from "../../constants";
import styled from "styled-components";
import { navigate } from "@reach/router";
import { LocalizedString } from "src/i18n";
import { useStore } from "simstate";
import { NavStore } from "src/layouts/nav/NavStore";
import HeaderIcon from "./HeaderIcon";
import { MenuFoldOutlined, MenuUnfoldOutlined } from "@ant-design/icons";
import { LanguageSelector } from "./LanguageSelector";
import { UserIndicator } from "./UserIndicator";
import { Logo } from "src/layouts/nav/Header/Logo";
import { ScopeIndicator } from "src/layouts/nav/Header/ScopeIndicator";
import { HeaderCompactMenu, HeaderNavMenu } from "src/layouts/nav/Header/HeaderMenus";
const { Header: AntdHeader } = Layout;


export const MarginedDiv = styled.div<{ margin?: number }>`

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



const FixedMenu: React.FC<{}> = () => (
  <HeaderCompactMenu>
    <UserIndicator />
    <LanguageSelector />
  </HeaderCompactMenu>
)

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
        <HeaderCompactMenu>
          <ScopeIndicator />
        </HeaderCompactMenu>
      </MarginedDiv>
      <MarginedDiv margin={0}>
        <MediaQuery minWidth={layoutConstants.menuBreakpoint}>
          {(matches) => matches
            ? <>
              <HeaderNavMenu vertical={false} selectedKeys={selectedKeys} to={navigate} />
              <FixedMenu />
            </>
            : <>
              <FixedMenu />
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
        </MediaQuery>
      </MarginedDiv>
    </Head>
  );
}


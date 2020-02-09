import React, { useCallback, useMemo, MouseEventHandler } from "react";
import { NavStore } from "../NavStore";
import { Layout, Menu } from "antd";
import { NavItemProps } from "../NavItemProps";
import styled from "styled-components";
import { antdBreakpoints, layoutConstants } from "../../constants";
import BodyMask from "./BodyMask";
import { useStore } from "simstate";
import { flatten, arrayContainsElement, removeFalsy } from "src/utils/Arrays";
import { LocalizedString } from "src/i18n";
import { navigate, Link as RouterLink } from "@reach/router";

const {Sider} = Layout;
const {SubMenu} = Menu;

const breakpoint = "lg";

interface Props {

}

const StyledSider = styled(Sider)`

  @media (max-width: ${antdBreakpoints[breakpoint]}px ) {
    position:absolute;
    height: 100%;
    z-index:5;

    body, html {
      overflow-x: hidden;
      overflow-y: auto;
    }

  }

  .ant-menu-item:first-child {
    margin-top: 0px;
  }

`;

const Link: React.FC<{
  path: string; Icon: React.ComponentType; textId: string;
  onClick: (path: string) => (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => void;
}> = (props) => {
  const {path, Icon, textId, onClick} = props;
  return <RouterLink to={path} title={path}>
    <Icon />
    <LocalizedString id={textId}/>
  </RouterLink>;
};

export const SideNav: React.FC<Props> = (props) => {
  const navStore = useStore(NavStore);

  const onBreakpoint = useCallback((broken: boolean) => {
    // if broken, big to small. collapse the sidebar
    // if not, small to big, expand the sidebar
    navStore.setSidebarCollapsed(broken);
  }, [navStore]);

  const selectedKeys = useMemo(() =>{
    // root selected keys
    const path = navStore.location.location.pathname;
    const rootSelected = navStore.sidenavs.filter((x) => x.match(path));

    // children selected keys
    const childrenSelected = flatten(rootSelected.map((x) => {
      if (arrayContainsElement(x.children)) {
        return (x.children ?? []).filter((child) => child.match(path));
      } else {
        return [];
      }
    }));

    return flatten([rootSelected, childrenSelected]).map((x) => x.path);
  }, [navStore]);

  const onClick = (path: string) => (e: React.MouseEvent) => {
    e.preventDefault();
    navigate(path)
    if (window.innerWidth <= antdBreakpoints[breakpoint]) {
      navStore.setSidebarCollapsed(true);
    }
  };

  if (!navStore.hasSider) {
    return null;
  }
  return (
    <>
      <BodyMask sidebarShown={!navStore.sidebarCollapsed} breakpoint={antdBreakpoints[breakpoint]}/>
      <StyledSider
        onBreakpoint={onBreakpoint}
        collapsed={navStore.sidebarCollapsed}
        collapsedWidth={0}
        breakpoint={breakpoint}
        trigger={null}>
        <Menu
          mode="inline"
          selectedKeys={selectedKeys}
          // theme={'dark'}
          style={{height: "100%", borderRight: 0}}
        >
          {navStore.sidenavs
            .map((x) => {
              const subs = x.children ?? [];
              if (!arrayContainsElement(subs)) {
                return <Menu.Item key={x.path}>
                  <Link path={x.path} textId={x.textId} Icon={x.Icon} onClick={onClick}/>
                </Menu.Item>;
              } else {
                return (
                  <SubMenu key={x.path} title={
                    <span>
                      {React.createElement(x.Icon)}
                      <span><LocalizedString id={x.textId}/></span>
                    </span>}
                  >
                    {subs.map((sub) =>
                      <Menu.Item key={sub.path}>
                        <Link path={sub.path} textId={sub.textId} Icon={sub.Icon} onClick={onClick}/>
                      </Menu.Item>,
                    )}
                  </SubMenu>
                );
              }

            },
            )}
        </Menu>
      </StyledSider>
    </>
  );
}


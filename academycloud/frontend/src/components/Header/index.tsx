import React, { useCallback } from "react";
import { Dropdown, Layout, Menu, Popover } from "antd";
import { BarsOutlined } from "@ant-design/icons";
import { mainNavs } from "./mainNavs";
import MediaQuery from "react-responsive";
import { layoutConstants } from "../../layouts/constants";
import styled from "styled-components";
import { Location, navigate, Link } from "@reach/router";
import {ReactComponent as LogoSvg} from "src/assets/logo-horizontal.svg";
import { LocalizedString } from "src/i18n";
import { useStore } from "simstate";
import { NavStore } from "src/layouts/nav/NavStore";
import HeaderIcon from "./HeaderIcon";
import { MenuFoldOutlined, MenuUnfoldOutlined }  from "@ant-design/icons";
const { Header: AntdHeader } = Layout;

interface Props {

}

export const MarginedDiv = styled.div`

  display: flex;
  align-items: center;
  & > * {
    margin-right: 4px;
    margin-left: 4px;
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

export const Header: React.FunctionComponent = () => {
  const navStore = useStore(NavStore);

  const collapse = useCallback(() => {
    navStore.setSidebarCollapsed(!navStore.sidebarCollapsed);
  }, [navStore]);

  return (
    <Location>
      {({ location }) => {
        const selectedKeys = mainNavs.filter((x) => x.match(location.pathname)).map((x) => x.path);

        return (
          <Head>
            <MarginedDiv>
              {navStore.hasSider
                ? <a onClick={collapse}>
                  <HeaderIcon
                    Icon={navStore.sidebarCollapsed ? MenuFoldOutlined : MenuUnfoldOutlined}
                  />
                </a>
                : null}
              <StyledLogo onClick={() => navigate("/resources")}/>
            </MarginedDiv>
            <MarginedDiv>
              <MediaQuery minWidth={layoutConstants.menuBreakpoint}>
                {(matches) => matches
                  ? <>
                    <HeaderNavMenu vertical={false} selectedKeys={selectedKeys} to={navigate} />
                  </>
                  : <>

                    <Dropdown overlay={<HeaderNavMenu
                      vertical={true}
                      selectedKeys={selectedKeys}
                      to={navigate}
                    />} trigger={["click"]}>
                      <BarsOutlined />
                    </Dropdown>
                  </>
                }
              </MediaQuery>
            </MarginedDiv>
          </Head>
        );
      }}
    </Location>
  );
}


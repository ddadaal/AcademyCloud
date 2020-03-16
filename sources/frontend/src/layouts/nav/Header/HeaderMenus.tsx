import React from "react";
import { Menu } from "antd";
import { layoutConstants } from 'src/layouts/constants';
import { mainNavs, selectMainNavs } from "src/layouts/nav/Header/mainNavs";
import { Localized } from "src/i18n";
import styled from "styled-components";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";

export const dropdownMenuStyle: React.CSSProperties = {
  width: "256px",
  textAlign: "center",
};

export const horizontalMenuStyle: React.CSSProperties = {
  lineHeight: `${layoutConstants.headerHeight}px`,
};

const CompactMenuItem = styled(Menu.Item)`
    padding: 0 8px;
`;

export function HeaderNavMenu(props: {
  vertical: boolean; selectedKeys: string[]; to(path: string): void;
}) {
  const { vertical, selectedKeys, to } = props;

  const { user } = useStore(UserStore);

  return (
    <Menu theme={"dark"}
      mode={vertical ? "vertical" : "horizontal"}
      selectedKeys={selectedKeys}
      style={vertical ? dropdownMenuStyle : horizontalMenuStyle}>
      {selectMainNavs(user?.scope).map((x) =>
        <Menu.Item
          key={x.path}
          onClick={() => to(x.path)}>
          {React.createElement(x.Icon)}
          <Localized id={x.textId} />
        </Menu.Item>,
      )}
    </Menu>
  );
}

export const HeaderCompactMenu: React.FC = ({ children }) => {

  return (
    <Menu theme="dark" mode="horizontal" style={horizontalMenuStyle} selectable={false}>
      {React.Children.map(children, (child, i) => (
        <CompactMenuItem key={i}>
          {child}
        </CompactMenuItem>
      ))}
    </Menu>
  )


}

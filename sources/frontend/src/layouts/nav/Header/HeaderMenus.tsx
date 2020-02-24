import React from "react";
import { Menu } from "antd";
import { layoutConstants } from 'src/layouts/constants';
import { mainNavs } from "src/layouts/nav/Header/mainNavs";
import { Localized } from "src/i18n";
import styled from "styled-components";

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

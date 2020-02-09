import React from "react";
import styled from "styled-components";
import HeaderItemContainer from "./HeaderItemContainer";
import { layoutConstants } from "src/layouts/constants";
import { DashboardOutlined } from "@ant-design/icons";

interface Props  {
  Icon: IconType;
  onClick?: () => void;
}

// export const HeaderColoredIcon = styled(Icon)`
//   color: ${layoutConstants.headerIconColor};
// `;

const iconStyle = { color: layoutConstants.headerIconColor};


export default function HeaderIcon(props: Props) {

  const { onClick, Icon } = props;

  return (
    <HeaderItemContainer onClick={onClick}>
      <Icon style={iconStyle}/>
    </HeaderItemContainer>
  );
}

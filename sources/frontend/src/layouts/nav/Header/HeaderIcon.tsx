import React from "react";
import { HeaderItemContainer } from "./HeaderItemContainer";
import { layoutConstants } from "src/layouts/constants";

interface Props  {
  Icon: IconType;
  onClick?: () => void;
}

// export const HeaderColoredIcon = styled(Icon)`
//   color: ${layoutConstants.headerIconColor};
// `;

const iconStyle = { color: layoutConstants.headerIconColor};


export const HeaderIcon = React.forwardRef((props: Props, ref) => {

  const { onClick, Icon } = props;

  return (
    <HeaderItemContainer onClick={onClick}>
      <Icon style={iconStyle}/>
    </HeaderItemContainer>
  );
});

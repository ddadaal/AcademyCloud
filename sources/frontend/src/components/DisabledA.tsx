import React from "react";
import { Tooltip } from "antd";

interface Props {
  onClick?: () => void;
  disabled?: boolean;
  message?: React.ReactNode;
}

export const DisabledA: React.FC<Props> = ({ onClick, disabled, message, children }) => {

  if (!disabled) {
    return <a onClick={onClick}>{children}</a>;
  }

  const inner = (
    <span>{children}</span>
  );

  if (message) {
    return (
      <Tooltip overlay={message}>
        {inner}
      </Tooltip>
    );
  } else {
    return inner;
  }

}

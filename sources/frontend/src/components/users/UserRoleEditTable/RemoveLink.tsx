import React from "react";
import { Popconfirm } from "antd";
import { Localized, lang } from "src/i18n";
import { User } from "src/models/User";
import { DisabledA } from "src/components/DisabledA";

interface Props {
  user: User;
  onRemove: (user: User) => void;
  disabled: boolean;
}

const root = lang.components.users;

export const RemoveLink: React.FC<Props> = ({ user, onRemove, disabled }) => {

  return (
    <Popconfirm
      disabled={disabled}
      title={<Localized id={root.remove.prompt} />}
      onConfirm={() => onRemove(user)}>
      <DisabledA disabled={disabled}>
        <Localized id={root.remove.link} />
      </DisabledA>
    </Popconfirm>
  );
}


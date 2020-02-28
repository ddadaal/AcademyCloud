import React from "react";
import { Popconfirm } from "antd";
import { Localized, lang } from "src/i18n";
import { User } from "src/models/User";
import { DisabledA } from "src/components/DisabledA";

interface Props {
  user: User;
  onSet: (user: User) => void;
  disabled: boolean;
}

const root = lang.components.users.setAsPayUser;

export const SetAsPayUserLink: React.FC<Props> = ({ user, onSet, disabled: disabled }) => {

  return (
    <Popconfirm
      disabled={disabled}
      title={<Localized id={root.prompt} />}
      onConfirm={() => onSet(user)}>
      <DisabledA disabled={disabled}>
        <Localized id={root.link} />
      </DisabledA>
    </Popconfirm>
  );
}


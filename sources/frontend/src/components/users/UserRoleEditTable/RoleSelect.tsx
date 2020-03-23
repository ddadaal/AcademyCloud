import React from "react";
import { UserRole } from 'src/models/Scope';
import { Select } from "antd";
import { lang, Localized } from "src/i18n";

const root = lang.components.users.role;

interface Props {
  value?: UserRole;
  onChange?: (user: UserRole) => void;
  disabled?: boolean;
}

export const RoleSelect: React.FC<Props> = ({
  value = UserRole.Admin,
  onChange = () => { },
  disabled = false,
}) => {

  return (
    <Select disabled={disabled} value={value} onChange={(e) => onChange(e)}>
      <Select.Option value="Admin"><Localized id={root.Admin} /></Select.Option>
      <Select.Option value="Member"><Localized id={root.Member} /></Select.Option>
    </Select>
  );

}

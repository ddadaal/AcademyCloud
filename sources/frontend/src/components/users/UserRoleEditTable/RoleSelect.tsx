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
  value = "admin",
  onChange = () => { },
  disabled = false,
}) => {

  return (
    <Select disabled={disabled} value={value} onChange={(e) => onChange(e)}>
      <Select.Option value="admin"><Localized id={root.admin} /></Select.Option>
      <Select.Option value="member"><Localized id={root.member} /></Select.Option>
    </Select>
  );

}

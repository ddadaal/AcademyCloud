import React from "react";
import { User } from "src/models/User";
import { lang } from "src/i18n";

interface Props {
  selected: User[];
  allUsers: User[];
  onChange: (users: User[]) => void;
}

const root = lang.components.users;

export const UsersSelectionMenu: React.FC<Props> = ({ selected, onChange }) => {
  return <div>123</div>;
}

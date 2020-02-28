import React, { useState, useCallback } from "react";
import { User } from 'src/models/User';
import { UserRole } from "src/models/Scope";
import { lang, Localized } from "src/i18n";
import { AddButton } from "src/components/users/UserRoleEditTable/AddButton";
import { ExistingTable } from "src/components/users/UserRoleEditTable/ExistingTable";
import { mergeAdminAndMember } from "src/components/users/UserWithRole";

interface Props {
  admins: User[];
  members: User[];

  onAdd: (userId: string, role: UserRole) => Promise<void>;
  onRoleChange: (userId: string, role: UserRole) => Promise<void>;
  onRemove: (userId: string) => Promise<void>;

  getAccessibleUsers: () => Promise<User[]>;
}

export const UserRoleEditTable: React.FC<Props> = (props) => {
  const { admins, members, onAdd, onRoleChange, getAccessibleUsers, onRemove } = props;

  const [allUsers, setAllUsers] = useState(() => mergeAdminAndMember(admins, members));

  const handleAdd = useCallback(async (user: User, role: UserRole) => {
    await onAdd(user.id, role);
    setAllUsers((users) => [...users, { ...user, role }]);
  }, [onAdd]);

  const handleRemove = useCallback(async (id: string) => {
    await onRemove(id);
    setAllUsers((users) => users.filter((x) => x.id !== id));
  }, [onRemove]);

  const getAccessibleUsersExceptAlreadyJoined = useCallback(async () => {
    const users = await getAccessibleUsers();

    return users.filter((u1) => allUsers.every((u2) => u2.id !== u1.id));
  }, [allUsers, getAccessibleUsers])

  return (
    <div>
      <AddButton onAdd={handleAdd} getUsers={getAccessibleUsersExceptAlreadyJoined} />
      <ExistingTable allUsers={allUsers} onRoleChange={onRoleChange} onRemove={handleRemove} />
    </div>
  )
}

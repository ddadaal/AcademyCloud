import React, { useState, useCallback } from "react";
import { User } from 'src/models/User';
import { UserRole } from "src/models/Scope";
import { AddButton } from "src/components/users/UserRoleEditTable/AddButton";
import { ExistingTable } from "src/components/users/UserRoleEditTable/ExistingTable";

interface Props {
  admins: User[];
  members: User[];
  payUser: User;

  onAdd: (userId: string, role: UserRole) => Promise<void>;
  onRoleChange: (userId: string, role: UserRole) => Promise<void>;
  onRemove: (userId: string) => Promise<void>;
  onPayUserSet: (userId: string) => Promise<void>;

  getAccessibleUsers: () => Promise<User[]>;
}

export const UserRoleEditTable: React.FC<Props> = (props) => {
  const { admins, members, onAdd, onRoleChange, getAccessibleUsers, onRemove, payUser, onPayUserSet } = props;

  const [allUsers, setAllUsers] = useState(() => ({ admins, members, payUser }));

  const handleAdd = useCallback(async (user: User, role: UserRole) => {
    await onAdd(user.id, role);
    if (role === "admin") {
      setAllUsers((users) => ({ ...users, admins: [...users.admins, user ]}));
    } else {
      setAllUsers((users) => ({ ...users, members: [...users.members, user ]}));
    }
  }, [onAdd]);

  const handleRemove = useCallback(async (user: User) => {
    await onRemove(user.id);
    setAllUsers((users) => ({
      payUser: users.payUser,
      admins: users.admins.filter((x) => x.id !== user.id),
      members: users.members.filter((x) => x.id !== user.id),
    }));
  }, [onRemove]);

  const handleSetPayUser = useCallback(async (user: User) => {
    await onPayUserSet(user.id);
    setAllUsers((users) => ({ ...users, payUser: user }));
  }, [onPayUserSet]);

  const getAccessibleUsersExceptAlreadyJoined = useCallback(async () => {
    const users = await getAccessibleUsers();

    return users.filter((u1) =>
      [...allUsers.members, ...allUsers.admins].every((u2) => u2.id !== u1.id));
  }, [allUsers, getAccessibleUsers])

  return (
    <div>
      <AddButton onAdd={handleAdd} getUsers={getAccessibleUsersExceptAlreadyJoined} />
      <ExistingTable
        payUser={allUsers.payUser}
        members={allUsers.members}
        admins={allUsers.admins}
        onRoleChange={onRoleChange}
        onRemove={handleRemove}
        onPayUserSet={handleSetPayUser}
      />
    </div>
  )
}

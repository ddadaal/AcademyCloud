import React, { useState, useCallback } from "react";
import { User } from 'src/models/User';
import { UserRole } from "src/models/Scope";
import { AddButton } from "src/components/users/UserRoleEditTable/AddButton";
import { ExistingTable } from "src/components/users/UserRoleEditTable/ExistingTable";
import { Resources } from "src/models/Resources";

interface Props {
  admins: User[];
  members: User[];
  payUser: User;

  onAdd: (userId: string, role: UserRole) => Promise<void>;
  onRoleChange: (userId: string, role: UserRole) => Promise<void>;
  onRemove: (userId: string) => Promise<void>;
  onPayUserSet: (userId: string) => Promise<void>;

  userResources?: { [userId: string]: Resources };
  onResourcesChange?: (userId: string, resources: Resources) => Promise<void>;
  getAvailableQuota?: (userId: string) => Promise<{ used: Resources; total: Resources }>;

  getAccessibleUsers: () => Promise<User[]>;
}

export const UserRoleEditTable: React.FC<Props> = (props) => {
  const { admins, members, onAdd, onRoleChange, getAccessibleUsers, onRemove, payUser, onPayUserSet, onResourcesChange, getAvailableQuota } = props;

  const [allUsers, setAllUsers] = useState(() => ({ admins, members, payUser }));
  const [userResources, setUserResources] = useState(props.userResources);

  const handleAdd = useCallback(async (user: User, role: UserRole) => {
    await onAdd(user.id, role);
    if (role === UserRole.Admin) {
      setAllUsers((users) => ({ ...users, admins: [...users.admins, user] }));
    } else {
      setAllUsers((users) => ({ ...users, members: [...users.members, user] }));
    }
    setUserResources((resources) => ({ ...resources, [user.id]: { cpu: 0, memory: 0, storage: 0 } }));
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

  const handleRoleChange = useCallback(async (user: User, role: UserRole) => {
    await onRoleChange(user.id, role);
    if (role === UserRole.Admin) {
      setAllUsers((users) => ({
        ...users,
        admins: [...users.admins, user],
        members: users.members.filter((x) => x.id !== user.id),
      }));
    } else {
      setAllUsers((users) => ({
        ...users,
        admins: users.admins.filter((x) => x.id !== user.id),
        members: [...users.members, user],
      }));
    }
  }, [onRoleChange]);

  const handleResourcesChange = useCallback(async (user: User, resources: Resources) => {
    if (onResourcesChange && userResources) {
      await onResourcesChange(user.id, resources);
      setUserResources((userResources) => ({ ...userResources, [user.id]: resources }));
    }
  }, [onResourcesChange, userResources]);

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
        userResources={userResources}
        onResourcesChange={onResourcesChange ? handleResourcesChange : undefined}
        getAvailableQuota={getAvailableQuota}
        onRoleChange={handleRoleChange}
        onRemove={handleRemove}
        onPayUserSet={handleSetPayUser}
      />
    </div>
  )
}

import React, { useCallback } from "react";
import { getApiService } from "src/apis";
import { useAsync } from "react-async";
import { UserRoleEditTable } from "src/components/users/UserRoleEditTable";
import { UserRole } from "src/models/Scope";
import { UsersService } from "src/apis/identity/UsersService";
import { Spin } from "antd";
import { UsersRoleViewTable } from "src/components/users/UsersRoleViewTable";
import { DomainsService } from "src/apis/identity/DomainsService";

interface Props {
  domainId: string;
  isAdmin: boolean;
  refreshToken: any;
}

const domainsService = getApiService(DomainsService);

const usersService = getApiService(UsersService);

const getAccessibleUsers = async () => {
  const resp = await usersService.getAccessibleUsers();

  return resp.users;
}


export const DomainUsersTable: React.FC<Props> = ({ domainId, isAdmin, refreshToken }) => {

  const getUsers = useCallback(async () => {
    return await domainsService.getUsersOfDomain(domainId);
  }, [domainId]);

  const { data, isPending } = useAsync({ promiseFn: getUsers, watch: refreshToken });

  const onAdd = useCallback(async (userId: string, role: UserRole) => {
    await domainsService.addUserToDomain(domainId, userId, role);
  }, [domainId]);

  const onRoleChange = useCallback(async (userId: string, role: UserRole) => {
    await domainsService.changeUserRole(domainId, userId, role);
  }, [domainId]);

  const onRemove = useCallback(async (userId: string) => {
    await domainsService.removeUser(domainId, userId);
  }, [domainId]);

  const onPayUserSet = useCallback(async (userId: string) => {
    await domainsService.setPayUser(domainId, userId);
  }, [domainId]);

  if (isPending) {
    return (<Spin spinning={true} />);
  }

  if (isAdmin) {
    return (
      <UserRoleEditTable
        admins={data!!.admins}
        members={data!!.members}
        payUser={data!!.payUser}
        onAdd={onAdd}
        onRoleChange={onRoleChange}
        onRemove={onRemove}
        onPayUserSet={onPayUserSet}
        getAccessibleUsers={getAccessibleUsers}
      />
    );
  } else {
    return (
      <UsersRoleViewTable
        admins={data!!.admins}
        members={data!!.members}
      />
    )
  }

}

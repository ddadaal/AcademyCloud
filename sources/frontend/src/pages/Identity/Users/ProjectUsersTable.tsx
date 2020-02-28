import React, { useCallback } from "react";
import { getApiService } from "src/apis";
import { ProjectsService } from "src/apis/identity/ProjectsService";
import { useAsync } from "react-async";
import { UserRoleEditTable } from "src/components/users/UserRoleEditTable";
import { UserRole } from "src/models/Scope";
import { UsersService } from "src/apis/identity/UsersService";
import { Spin } from "antd";
import { UsersRoleViewTable } from "src/components/users/UsersRoleViewTable";

interface Props {
  projectId: string;
  isAdmin: boolean;
  refreshToken: any;
}

const projectsService = getApiService(ProjectsService);

const usersService = getApiService(UsersService);

const getAccessibleUsers = async () => {
  const resp = await usersService.getAccessibleUsers();

  return resp.users;
}


export const ProjectUsersTable: React.FC<Props> = ({ projectId, isAdmin, refreshToken }) => {

  const getUsers = useCallback(async () => {
    return await projectsService.getUsersOfProject(projectId);
  }, [projectId]);

  const { data, isPending } = useAsync({ promiseFn: getUsers, watch: refreshToken });

  const onAdd = useCallback(async (userId: string, role: UserRole) => {
    await projectsService.addUserToProject(projectId, userId, role);
  }, [projectId]);

  const onRoleChange = useCallback(async (userId: string, role: UserRole) => {
    await projectsService.changeUserRole(projectId, userId, role);
  }, [projectId]);

  const onRemove = useCallback(async (userId: string) => {
    await projectsService.removeUser(projectId, userId);
  }, [projectId]);


  if (isPending) {
    return (<Spin spinning={true} />);
  }

  if (isAdmin) {
    return (
      <UserRoleEditTable
        admins={data!!.admins}
        members={data!!.members}
        onAdd={onAdd}
        onRoleChange={onRoleChange}
        onRemove={onRemove}
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

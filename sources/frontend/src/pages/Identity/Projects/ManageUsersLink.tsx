import React, { useState, useCallback } from "react";
import { Project } from 'src/models/Project';
import { lang, Localized } from "src/i18n";
import { Modal, Button } from "antd";
import { UserRoleEditTable } from "src/components/users/UserRoleEditTable";
import { getApiService } from "src/apis";
import { UsersService } from "src/apis/identity/UsersService";
import { ProjectsService } from "src/apis/identity/ProjectsService";
import { UserRole } from "src/models/Scope";
import { QuotaService } from "src/apis/expenses/QuotaService";
import { BillType, BillSubjectType } from "src/models/Billings";

interface Props {
  project: Project;
  reload: () => void;
}

const root = lang.identity.projects.table.manageUsers;

const usersService = getApiService(UsersService);

const getAccessibleUsers = () => usersService.getAccessibleUsers().then((x) => x.users);

const projectsService = getApiService(ProjectsService);

const quotaService = getApiService(QuotaService);

export const ManageUsersLink: React.FC<Props> = ({ project, reload }) => {

  const [modalShown, setModalShown] = useState(false);

  const [changed, setChanged] = useState(false);

  const onAdd = useCallback(async (userId: string, role: UserRole) => {
    await projectsService.addUserToProject(project.id, userId, role);
    setChanged(true);
  }, [project]);
  const onRoleChange = useCallback(async (userId: string, role: UserRole) => {
    await projectsService.changeUserRole(project.id, userId, role);
    setChanged(true);
  }, [project]);

  const onRemove = useCallback(async (userId: string) => {
    await projectsService.removeUserFromProject(project.id, userId);
    setChanged(true);
  }, [project]);

  const onPayUserSet = useCallback(async (userId: string) => {
    await projectsService.setPayUser(project.id, userId);
    setChanged(true);
  }, [project]);

  const close = useCallback(() => {
    setModalShown(false);
    if (changed) {
      reload();
    }
  }, [changed]);

  const getAvailableQuota = useCallback(async (userId: string) => {
    return await quotaService.getQuotaStatus(BillSubjectType.Project, project.id);
  }, [project]);

  return (
    <>
      <a onClick={() => setModalShown(true)}>
        <Localized id={root.link} />
      </a>
      <Modal
        title={<Localized id={root.link} />}
        visible={modalShown}
        onOk={close}
        onCancel={close}
        footer={[
          <Button key="close" type="primary" onClick={close}>
            <Localized id={changed ? root.closeAndRefresh : root.close} />
          </Button>
        ]}
        destroyOnClose={true}
      >
        <UserRoleEditTable
          admins={project.admins}
          members={project.members}
          payUser={project.payUser}
          onAdd={onAdd}
          onRoleChange={onRoleChange}
          onRemove={onRemove}
          onPayUserSet={onPayUserSet}
          getAccessibleUsers={getAccessibleUsers}
          getAvailableQuota={getAvailableQuota}
        />
      </Modal>
    </>

  )

}

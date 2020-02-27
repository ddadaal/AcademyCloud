import React, { useState } from "react";
import { Project } from 'src/models/Project';
import { lang, Localized } from "src/i18n";
import { Modal } from "antd";
import { UserRoleEditTable } from "src/components/users/UserRoleEditTable";
import { getApiService } from "src/apis";
import { UsersService } from "src/apis/identity/UsersService";
import { ProjectsService } from "src/apis/identity/ProjectsService";

interface Props {
  project: Project;
  reload: () => void;
}

const root = lang.identity.projects.table.manageUsers;

const usersService = getApiService(UsersService);

const getAccessibleUsers = () => usersService.getAccessibleUsers().then((x) => x.users);

const projectsService = getApiService(ProjectsService);

export const ManageUsersLink: React.FC<Props> = ({ project, reload }) => {

  const [modalShown, setModalShown] = useState(false);



  return (
    <>
      <a onClick={() => setModalShown(true)}>
        <Localized id={root.link} />
      </a>
      <Modal
        title={<Localized id={root.link} />}
        visible={modalShown}
        onOk={() => setModalShown(false)}
        onCancel={() => setModalShown(false)}
        destroyOnClose={true}
      >
        <UserRoleEditTable
          admins={project.admins}
          members={project.members}
          onAdd={(userId, role) => projectsService.addUserToProject(project.id, userId, role)}
          onRoleChange={(userId, role) => projectsService.changeUserRole(project.id, userId, role)}
          onRemove={(userId) => projectsService.removeUser(project.id, userId)}
          getAccessibleUsers={getAccessibleUsers}
          refresh={reload}
        />
      </Modal>
    </>

  )

}

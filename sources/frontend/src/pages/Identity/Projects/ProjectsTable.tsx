import React, { useCallback } from "react";
import { getApiService } from "src/apis";
import { ProjectsService } from "src/apis/identity/ProjectsService";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { useAsync } from "react-async";
import { Table } from "antd";
import { lang, Localized } from "src/i18n";
import { ModalLink } from "src/components/ModalLink";
import { User } from "src/models/User";
import { UsersViewTable } from "src/components/users/UsersViewTable";
import { resourcesString, Resources } from "src/models/Resources";
import { ResourcesViewTable } from "src/components/resources/ResourcesViewTable";
import { UsersRoleViewTable } from "src/components/users/UsersRoleViewTable";
import { Project } from "src/models/Project";
import { EditLink } from "src/pages/Identity/Projects/EditLink";

interface Props {
  refreshToken: any;
}

const root = lang.identity.projects.table;

const service = getApiService(ProjectsService);

export const ProjectsTable: React.FC<Props> = (props) => {

  const userStore = useStore(UserStore);

  const { user } = userStore;

  const getAccessibleProjects = useCallback(async () => {
    const x = await service.getAccessibleProjects();
    return x.projects;
  }, [user]);

  const { data, isPending, reload } = useAsync({ promiseFn: getAccessibleProjects });

  return (
    <Table dataSource={data} loading={isPending}>
      <Table.Column title={<Localized id={root.id} />} dataIndex="id" />
      <Table.Column title={<Localized id={root.name} />} dataIndex="name" />
      <Table.Column title={<Localized id={root.active.title} />} dataIndex="active"
        render={(active: boolean) => <Localized id={root.active[String(active)]} />}
      />
      <Table.Column title={<Localized id={root.payUser} />} dataIndex="payUser"
        render={(payUser: User) => (
          <ModalLink modalTitle={<Localized id={root.payUser} />} modalContent={<UsersViewTable users={[payUser]} />}>
            {payUser.name}
          </ModalLink>
        )} />
      <Table.Column title={<Localized id={root.users} />}
        render={(_, project: Project) => (
          <ModalLink modalTitle={<Localized id={root.users} />} modalContent={<UsersRoleViewTable admins={project.admins} members={project.members} />}>
            {project.admins.length + project.members.length}
          </ModalLink>
        )} />
      <Table.Column title={<Localized id={root.resources} />} dataIndex="resources"
        render={(resources: Resources) => (
          <ModalLink modalTitle={<Localized id={root.resources} />} modalContent={<ResourcesViewTable resources={resources} />}>
            {resourcesString(resources)}
          </ModalLink>
        )} />
      // ensure its an domain admin
      {((user) && (!!user.scope.projectId) && (user.scope.role === "admin"))
        ? (
          <Table.Column title={<Localized id={root.actions} />}
            render={(_, project: Project) => (
              <EditLink project={project} reload={reload} />
            )} />
        )
        : null}
    </Table>
  );
}

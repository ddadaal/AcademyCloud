import React, { useCallback } from "react";
import { getApiService } from "src/apis";
import { ProjectsService } from "src/apis/identity/ProjectsService";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { useAsync } from "react-async";
import { Table, Divider } from "antd";
import { lang, Localized } from "src/i18n";
import { ModalLink } from "src/components/ModalLink";
import { User } from "src/models/User";
import { UsersViewTable } from "src/components/users/UsersViewTable";
import { resourcesString, Resources } from "src/models/Resources";
import { ResourcesViewTable } from "src/components/resources/ResourcesViewTable";
import { UsersRoleViewTable } from "src/components/users/UsersRoleViewTable";
import { Project } from "src/models/Project";
import { EditLink } from "src/pages/Identity/Projects/EditLink";
import { Scope, isDomainAdmin } from "src/models/Scope";
import { DeleteProjectLink } from "src/pages/Identity/Projects/DeleteProjectLink";
import { ResourcesModalLink } from 'src/components/resources/ResourcesModalLink';

interface Props {
  refreshToken: any;
  scope: Scope;
}

const root = lang.identity.projects.table;

const service = getApiService(ProjectsService);

export const ProjectsTable: React.FC<Props> = ({ refreshToken, scope }) => {

  const getAccessibleProjects = useCallback(async () => {
    const x = await service.getAccessibleProjects();
    return x.projects;
  }, [scope]);

  const { data, isPending, reload } = useAsync({
    promiseFn: getAccessibleProjects,
    watch: refreshToken,
  });

  return (
    <Table dataSource={data} loading={isPending} rowKey="id">
      <Table.Column title={<Localized id={root.name} />} dataIndex="name" />
      <Table.Column title={<Localized id={root.active.title} />} dataIndex="active"
        render={(active: boolean) => <Localized id={root.active[String(active)]} />}
      />
      {isDomainAdmin(scope)
        ? (
          <Table.Column title={<Localized id={root.payUser} />} dataIndex="payUser"
            render={(payUser: User) => (
              <ModalLink modalTitle={<Localized id={root.payUser} />} modalContent={<UsersViewTable users={[payUser]} />}>
                {payUser.name}
              </ModalLink>
            )} />
        ) : null}
      <Table.Column title={<Localized id={root.users} />}
        render={(_, project: Project) => (
          <ModalLink modalTitle={<Localized id={root.users} />} modalContent={<UsersRoleViewTable admins={project.admins} members={project.members} />}>
            {project.admins.length + project.members.length}
          </ModalLink>
        )} />
      <Table.Column title={<Localized id={root.quota} />} dataIndex="quota"
        render={(quota: Resources) => <ResourcesModalLink resources={quota} />} />
      {isDomainAdmin
        ? (
          <Table.Column title={<Localized id={root.actions} />}
            render={(_, project: Project) => (
              <span>
                <EditLink project={project} reload={reload} />
                <Divider type="vertical" />
                <DeleteProjectLink project={project} reload={reload} />
              </span>
            )} />
        )
        : null}
    </Table>
  );
}

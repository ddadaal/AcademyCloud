import React from "react";
import { getApiService } from "src/apis";
import { DomainsService } from "src/apis/identity/DomainsService";
import { Table, Modal } from "antd";
import { LocalizedString, lang } from "src/i18n";
import { useAsync } from "react-async";
import { UsersViewTable } from "src/components/users/UsersViewTable";
import { User } from "src/models/User";
import { Resources } from "src/models/Resources";
import { ResourcesViewTable } from "src/components/resources/ResourcesViewTable";

const root = lang.identity.domains;

const service = getApiService(DomainsService);

const getDomains = () => {
  return service.getDomains();
}

export const DomainsTable: React.FC = () => {

  const { data, isPending } = useAsync({ promiseFn: getDomains });

  const [api, contextHolder] = Modal.useModal();

  return (
    <>
      {contextHolder}
      <Table dataSource={data?.domains} loading={isPending}>
        <Table.Column title={<LocalizedString id={root.id} />} dataIndex="id" key="domainId" />
        <Table.Column title={<LocalizedString id={root.name} />} dataIndex="name" key="domainName" />
        <Table.Column title={<LocalizedString id={root.admins} />} dataIndex="admins" key="admins"
          render={(admins: User[]) => {
            return (
              <a onClick={() => api.info({ icon: null, content: <UsersViewTable users={admins} /> })}>
                {admins.length <= 1 ? admins[0].name : admins.length}
              </a>
            )
          }} />
        <Table.Column title={<LocalizedString id={root.resources} />} dataIndex="resources" key="resources"
          render={(resources: Resources) => {
            return (
              <a onClick={() => api.info({ icon: null, content: <ResourcesViewTable resources={resources} /> })}>
                {JSON.stringify(resources)}
              </a>
            )
          }} />
      </Table>
    </>
  )

}

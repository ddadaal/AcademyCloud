import React from "react";
import { getApiService } from "src/apis";
import { DomainsService } from "src/apis/identity/DomainsService";
import { Table, Divider } from "antd";
import { Localized, lang } from "src/i18n";
import { useAsync } from "react-async";
import { UsersViewTable } from "src/components/users/UsersViewTable";
import { Resources, resourcesString } from "src/models/Resources";
import { ResourcesViewTable } from "src/components/resources/ResourcesViewTable";
import { Domain } from "src/models/Domain";
import { User } from "src/models/User";
import { EditLink } from "src/pages/Identity/Domains/EditLink";
import { ModalLink } from "src/components/ModalLink";
import { DeleteDomainLink } from './DeleteDomainLink';
import { ResourcesModalLink } from "src/components/resources/ResourcesModalLink";

const root = lang.identity.domains;

const service = getApiService(DomainsService);

const getDomains = () => {
  return service.getDomains();
}

interface Props {
  refreshToken: any;
}

export const DomainsTable: React.FC<Props> = ({ refreshToken }) => {

  const { data, isPending, reload } = useAsync({ promiseFn: getDomains, watch: refreshToken });

  return (
    <Table dataSource={data?.domains} loading={isPending} rowKey="id">
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
      <Table.Column title={<Localized id={root.admins} />} dataIndex="admins"
        render={(admins: User[]) => (
          <ModalLink modalTitle={<Localized id={root.admins} />} modalContent={<UsersViewTable users={admins} />}>
            {admins.length <= 1 ? admins[0].name : admins.length}
          </ModalLink>
        )} />
      <Table.Column title={<Localized id={root.quota} />} dataIndex="quota"
        render={(resources: Resources) => <ResourcesModalLink resources={resources} />} />
      <Table.Column title={<Localized id={root.actions} />}
        dataIndex="id"
        render={(_, domain: Domain) => (
          <span>
            <EditLink domain={domain} reload={reload} />
            <Divider type="vertical" />
            <DeleteDomainLink domain={domain} reload={reload} />
          </span>
        )} />
    </Table>
  );

}

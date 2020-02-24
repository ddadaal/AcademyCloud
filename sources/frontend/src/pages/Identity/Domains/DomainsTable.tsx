import React from "react";
import { getApiService } from "src/apis";
import { DomainsService } from "src/apis/identity/DomainsService";
import { Table, Modal } from "antd";
import { Localized, lang } from "src/i18n";
import { useAsync } from "react-async";
import { UsersViewTable } from "src/components/users/UsersViewTable";
import { Resources } from "src/models/Resources";
import { ResourcesViewTable } from "src/components/resources/ResourcesViewTable";
import { ModalStaticFunctions } from "antd/lib/modal/confirm";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { SetResourcesLink } from "src/pages/Identity/Domains/SetResourcesLink";
import { SetAdminLink } from "src/pages/Identity/Domains/SetAdminLink";
import { Domain } from "src/models/Domain";
import { User } from "src/models/User";

const root = lang.identity.domains;

const service = getApiService(DomainsService);

const getDomains = () => {
  return service.getDomains();
}

const ModalLink: React.FC<{
  api: Pick<ModalStaticFunctions, "info">;
  modalTitle: React.ReactNode;
  modalContent: React.ReactNode;
}> = ({ api, children, modalTitle, modalContent }) => {
  return (
    <a onClick={() => api.info({
      icon: null,
      title: <TitleText>{modalTitle}</TitleText>,
      content: modalContent
    })}>
      {children}
    </a>
  );
}

export const DomainsTable: React.FC = () => {

  const { data, isPending, reload } = useAsync({ promiseFn: getDomains });

  const [api, contextHolder] = Modal.useModal();

  return (
    <>
      {contextHolder}
      <Table dataSource={data?.domains} loading={isPending}>
        <Table.Column title={<Localized id={root.id} />} dataIndex="id" key="domainId" />
        <Table.Column title={<Localized id={root.name} />} dataIndex="name" key="domainName" />
        <Table.Column title={<Localized id={root.active.title} />} dataIndex="active" key="domainName"
          render={(active: boolean) => <Localized id={root.active[String(active)]} />}
        />
        <Table.Column title={<Localized id={root.payUser} />} dataIndex="payUser" key="payUser"
          render={(payUser: User) => (
            <ModalLink api={api} modalTitle={<Localized id={root.payUser} />} modalContent={<UsersViewTable users={[payUser]} />}>
              {payUser.name}
            </ModalLink>
          )} />
        <Table.Column title={<Localized id={root.admins} />} dataIndex="admins" key="admins"
          render={(admins: User[]) => (
            <ModalLink api={api} modalTitle={<Localized id={root.admins} />} modalContent={<UsersViewTable users={admins} />}>
              {admins.length <= 1 ? admins[0].name : admins.length}
            </ModalLink>
          )} />
        <Table.Column title={<Localized id={root.resources} />} dataIndex="resources" key="resources"
          render={(resources: Resources) => (
            <ModalLink api={api} modalTitle={<Localized id={root.resources} />} modalContent={<ResourcesViewTable resources={resources} />}>
              <Localized
                id={lang.components.resources.string}
                replacements={[resources.cpu, resources.memory, resources.storage]}
              />
            </ModalLink>
          )} />
        <Table.Column title={<Localized id={root.actions} />} key="domainId"
          render={(_, domain: Domain) => (
            <span>
              <SetAdminLink domain={domain} reload={reload} />
              {" | "}
              <SetResourcesLink domain={domain} reload={reload} />
            </span>
          )} />
      </Table>
    </>
  )

}

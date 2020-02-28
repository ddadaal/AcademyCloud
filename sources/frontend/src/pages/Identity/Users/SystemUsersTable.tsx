import React from "react";
import { useAsync } from "react-async";
import { getApiService } from "src/apis";
import { UsersService } from "src/apis/identity/UsersService";
import { Table } from "antd";
import { Localized, lang } from "src/i18n";
import { User } from "src/models/User";
import { RemoveUserLink } from "src/pages/Identity/Users/RemoveUserLink";

const { Column } = Table;

interface Props {
  refreshToken: any;
}

const root = lang.identity.users;

const service = getApiService(UsersService);

const getAccessibleUsers = async () => {
  const resp = await service.getAccessibleUsers();
  return resp.users;
}

export const SystemUsersTable: React.FC<Props> = ({ refreshToken }) => {

  const { data, isPending, reload } = useAsync({
    promiseFn: getAccessibleUsers,
    watch: refreshToken,
  });

  return (
    <Table dataSource={data} loading={isPending}>
      <Column title={<Localized id={root.id} />}
        dataIndex="id" key="id" />
      <Column title={<Localized id={root.name} />}
        dataIndex="name" key="name" />
      <Column title={<Localized id={root.active.title} />}
        dataIndex="active" key="id"
        render={(active: boolean) => (
          <Localized id={root.active[String(active)]} />
        )} />
      <Table.Column title={<Localized id={root.actions} />}
        render={(_, user: User) => (
          <RemoveUserLink user={user} isSystem={true} domainId="dummy" reload={reload} />
        )} />
    </Table>
  );
}

import React, { useCallback } from "react";
import { Table } from "antd";
import { ColumnProps } from "antd/lib/table";
import { lang, LocalizedString } from "src/i18n";
import { UserDomainAssignment } from "src/models/UserDomainAssignment";
import { useAsync } from "react-async";
import { getApiService } from "src/apis";
import { PersonalAccountService } from "src/apis/identity/PersonalAccountService";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";

const root = lang.identity.account.joinedDomains.table;

const columns = [
  {
    title: <LocalizedString id={root.id} />,
    dataIndex: "domainId",
    key: "domainId",
  },
  {
    title: <LocalizedString id={root.name} />,
    dataIndex: "domainName",
    key: "domainName",
  },
  {
    title: <LocalizedString id={root.role} />,
    dataIndex: "role",
    key: "role",
  }
] as ColumnProps<UserDomainAssignment>[];

const service = getApiService(PersonalAccountService);

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export function DomainsTable() {

  const userStore = useStore(UserStore);

  const getDomains = useCallback(async () => {
    return await service.getDomains();
  }, [userStore.user]);

  const { data, isPending } = useAsync({ promiseFn: getDomains });

  return (
    <Table columns={columns} dataSource={data?.domains} loading={isPending} />
  );
}


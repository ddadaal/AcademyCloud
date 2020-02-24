import React, { useCallback, useState, useMemo } from "react";
import { Table, Modal } from "antd";
import { ColumnProps } from "antd/lib/table";
import { lang, LocalizedString } from "src/i18n";
import { UserDomainAssignment } from "src/models/UserDomainAssignment";
import { useAsync } from "react-async";
import { getApiService } from "src/apis";
import { PersonalAccountService, ExitDomainsError } from "src/apis/identity/PersonalAccountService";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { Domain } from "domain";
import { HttpError } from "src/apis/HttpService";
import { DomainTableExitLink } from "src/pages/Identity/Account/JoinedDomains/DomainTableExitLink";

const root = lang.identity.account.joinedDomains.table;

const commonColumns = [
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
  },
] as ColumnProps<UserDomainAssignment>[];

const service = getApiService(PersonalAccountService);

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export function DomainsTable() {

  const userStore = useStore(UserStore);

  const getDomains = useCallback(async () => {
    return await service.getDomains();
  }, [userStore.user]);

  const { data, isPending, reload } = useAsync({ promiseFn: getDomains });

  const columns = useMemo(() => [
    ...commonColumns,
    {
      title: <LocalizedString id={root.actions} />,
      key: "actions",
      render: (_, domain: UserDomainAssignment) => (
        <DomainTableExitLink domain={domain} reload={reload} />
      )
    }
  ], [reload]);


  return (
    <Table columns={columns} dataSource={data?.domains} loading={isPending} />
  )
}


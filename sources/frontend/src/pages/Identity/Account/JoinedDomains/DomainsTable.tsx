import React, { useCallback, useMemo } from "react";
import { Table } from "antd";
import { ColumnProps } from "antd/lib/table";
import { lang, Localized } from "src/i18n";
import { UserDomainAssignment } from "src/models/UserDomainAssignment";
import { useAsync } from "react-async";
import { getApiService } from "src/apis";
import { PersonalAccountService } from "src/apis/identity/PersonalAccountService";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { DomainTableExitLink } from "src/pages/Identity/Account/JoinedDomains/DomainTableExitLink";

const root = lang.identity.account.joinedDomains.table;

const commonColumns = [
  {
    title: <Localized id={root.id} />,
    dataIndex: "domainId",
    key: "domainId",
  },
  {
    title: <Localized id={root.name} />,
    dataIndex: "domainName",
    key: "domainName",
  },
  {
    title: <Localized id={root.role} />,
    dataIndex: "role",
    key: "role",
  },
] as ColumnProps<UserDomainAssignment>[];

const service = getApiService(PersonalAccountService);

interface Props {
  refreshToken: any;
}

export const DomainsTable: React.FC<Props> = ({ refreshToken }) => {

  const userStore = useStore(UserStore);

  const getDomains = useCallback(async () => {
    return await service.getJoinedDomains();
  }, [userStore.user]);

  const { data, isPending, reload } = useAsync({
    promiseFn: getDomains,
    watch: refreshToken,
  });

  const columns = useMemo(() => [
    ...commonColumns,
    {
      title: <Localized id={root.actions} />,
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


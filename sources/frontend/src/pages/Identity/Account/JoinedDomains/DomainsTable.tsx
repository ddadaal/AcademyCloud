import React, { useCallback } from "react";
import { Table } from "antd";
import { lang, Localized } from "src/i18n";
import { UserDomainAssignment } from "src/models/UserDomainAssignment";
import { useAsync } from "react-async";
import { getApiService } from "src/apis";
import { AccountService } from "src/apis/identity/AccountService";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { ExitDomainLink } from "src/pages/Identity/Account/JoinedDomains/ExitDomainLink";

const root = lang.identity.account.joinedDomains.table;

const service = getApiService(AccountService);

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

  return (
    <Table dataSource={data?.domains} loading={isPending} >
      <Table.Column title={<Localized id={root.name} />  } dataIndex="domainName" />
      <Table.Column title={<Localized id={root.role} />  } dataIndex="role" />
      <Table.Column title={<Localized id={root.actions} />} render={(_, domain: UserDomainAssignment) => (
        <ExitDomainLink domain={domain} reload={reload} disabled={userStore.user?.scope.domainId === domain.domainId} />
      )}/>
    </Table>
  )
}


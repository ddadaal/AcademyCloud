import React from "react";
import { GetJoinableDomainsResponse, AccountService } from "src/apis/identity/AccountService";
import { getApiService } from "src/apis";
import { useAsync } from "react-async";
import { Table } from "antd";
import { Localized, lang } from "src/i18n";

const root = lang.identity.account.joinDomain;

const service = getApiService(AccountService);

const getJoinableDomains = async () => {
  const resp = await service.getJoinableDomains();
  return resp.domains;
}

type Domain = GetJoinableDomainsResponse["domains"][0];

export const JoinableDomainSelectionTable: React.FC<{
  value?: Domain;
  onChange?: (domain: Domain) => void;
}> = ({ value, onChange = () => { } }) => {

  const { isPending, data } = useAsync({
    promiseFn: getJoinableDomains,
  });

  return (
    <Table rowSelection={{
      type: "radio",
      selectedRowKeys: value ? [value.id] : [],
      onChange: (e) => onChange(data?.find((x) => e.includes(x.id)) ?? undefined!!),
    }} size="small" dataSource={data} loading={isPending} rowKey="id">
      <Table.Column title={<Localized id={root.name} />} dataIndex="name" />
    </Table>
  );
}

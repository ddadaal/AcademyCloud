import React from "react";
import { Table } from "antd";
import { getApiService } from "src/apis";
import { TransactionsService } from "src/apis/expenses/TransactionsService";
import { useAsync } from "react-async";
import { lang, Localized } from "src/i18n";

interface Props {
  refreshToken: any;
}

const root = lang.expenses.accountTransactions;

const service = getApiService(TransactionsService);

const getAccountTransactions = () => service.getAccountTransactions().then((x) => x.transactions);

export const TransactionsTable: React.FC<Props> = (props) => {

  const { data, isPending } = useAsync({ promiseFn: getAccountTransactions, watch: props.refreshToken });

  return (
    <Table dataSource={data} loading={isPending} rowKey="id">
      <Table.Column title={<Localized id={root.time} />} dataIndex="time" />
      <Table.Column title={<Localized id={root.amount} />} dataIndex="amount" />
      <Table.Column title={<Localized id={root.reason} />} dataIndex="reason" />
    </Table>
  )
}

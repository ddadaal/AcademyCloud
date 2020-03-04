import React from "react";
import { Table } from "antd";
import { lang, Localized } from "src/i18n";
import { TransactionTypeText } from "src/i18n/TransactionTypeText";
import { TransactionType, TransactionReason } from "src/models/TransactionReason";
import dayjs from "dayjs";
import { LocalizedDate } from "src/i18n/LocalizedDate";
import { OrgTransaction } from "src/models/OrgTransaction";

interface Props {
  data: OrgTransaction[] | undefined;
  loading?: boolean;
}

const root = lang.components.transactions.org;

const timeSorter = (a: string, b: string) => dayjs(a).isBefore(dayjs(b)) ? -1 : 1;

export const OrgTransactionTable: React.FC<Props> = ({ data, loading = false }) => {

  return (
    <Table loading={loading} dataSource={data} rowKey="id">
      <Table.Column title={<Localized id={root.time} />} dataIndex="time"
        sorter={timeSorter}
        defaultSortOrder="descend"
        render={(time: string) => <LocalizedDate dateTimeString={time} />}
      />
      <Table.Column title={<Localized id={root.payer} />} dataIndex="payerName" />
      <Table.Column title={<Localized id={root.receiver} />} dataIndex="receiverName" />
      <Table.Column title={<Localized id={root.amount} />} dataIndex="amount"
        render={(amount: number) => amount.toFixed(2)}
      />
      <Table.Column title={<Localized id={root.reason} />} dataIndex="reason"
        render={(reason: TransactionReason) => (
          <TransactionTypeText type={reason.type} />
        )} />
    </Table>
  )
}

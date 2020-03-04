import React from "react";
import { Table } from "antd";
import { lang, Localized } from "src/i18n";
import { TransactionTypeText } from "src/i18n/TransactionTypeText";
import { TransactionType, TransactionReason } from "src/models/TransactionReason";
import dayjs from "dayjs";
import { LocalizedDate } from "src/i18n/LocalizedDate";
import { AccountTransaction } from "src/models/AccountTransaction";

interface Props {
  data: AccountTransaction[];
}

const root = lang.components.transactions.account;

const timeSorter = (a: string, b: string) => dayjs(a).isBefore(dayjs(b)) ? -1 : 1;

export const AccountTransactionTable: React.FC<Props> = ({ data }) => {

  return (
    <Table dataSource={data} rowKey="id">
      <Table.Column title={<Localized id={root.time} />} dataIndex="time"
        sorter={timeSorter}
        defaultSortOrder="descend"
        render={(time: string) => <LocalizedDate dateTimeString={time} />}
      />
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

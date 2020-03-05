import React from "react";
import { Table, Tooltip } from "antd";
import { ResourcesModalLink } from "src/components/resources/ResourcesModalLink";
import { Resources } from "src/models/Resources";
import { LocalizedDate } from "src/i18n/LocalizedDate";
import { Localized, lang } from "src/i18n";
import { HistoryUsageBilling, HistoryAllocatedBilling } from "src/models/Billings";

const root = lang.components.billings.table;

interface Props {
  data: (HistoryAllocatedBilling | HistoryUsageBilling)[] | undefined;
  loading?: boolean;
  hasPayer?: boolean;
}

export const HistoryBillingsTable: React.FC<Props> = ({ data, hasPayer = true, loading }) => {
  return (
    <Table dataSource={data} loading={loading} rowKey="id">
      <Table.Column title={<Localized id={root.startTime} />} dataIndex="startTime"
        render={(time: string) => <LocalizedDate dateTimeString={time} />} />
      <Table.Column title={<Localized id={root.endTime} />} dataIndex="endTime"
        render={(time: string) => <LocalizedDate dateTimeString={time} />} />
      <Table.Column title={<Localized id={root.resources} />} dataIndex="resources"
        render={(resources: Resources) => <ResourcesModalLink resources={resources} />} />
      <Table.Column title={<Localized id={root.amount} />} dataIndex="amount"
        render={(amount: number) => amount.toFixed(2)} />
      {hasPayer ? (
        <Table.Column title={<Localized id={root.payer} />} dataIndex="payerName"
          render={(_, item: HistoryAllocatedBilling) => <Tooltip overlay={item.payerId}>{item.payerName}</Tooltip>} />
      ) : null}
    </Table>
  )
}

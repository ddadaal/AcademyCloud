import React from "react";
import { Table, Tooltip } from "antd";
import { ResourcesModalLink } from "src/components/resources/ResourcesModalLink";
import { Resources } from "src/models/Resources";
import { LocalizedDate } from "src/i18n/LocalizedDate";
import { lang, Localized } from "src/i18n";
import { BillType, HistoryAllocatedBilling, HistoryUsedBilling } from "src/models/Billings";

const root = lang.components.billings.table;

interface Props {
  data: (HistoryAllocatedBilling | HistoryUsedBilling)[] | undefined;
  loading?: boolean;
  type: BillType;
}

export const HistoryBillingsTable: React.FC<Props> = ({ data, type, loading }) => {
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
      {type === BillType.Allocated ? (
        <Table.Column title={<Localized id={root.payer} />} dataIndex="payerName"
          render={(_, item: HistoryAllocatedBilling) => <Tooltip overlay={item.payerId}><span>{item.payerName}</span></Tooltip>} />
      ) : null}
    </Table>
  )
}

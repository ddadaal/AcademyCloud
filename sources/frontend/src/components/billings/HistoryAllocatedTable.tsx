import React from "react";
import { HistoryAllocatedBilling } from "src/models/HistoryAllocatedBilling";
import { Table } from "antd";
import { ResourcesModalLink } from "src/components/resources/ResourcesModalLink";
import { Resources } from "src/models/Resources";
import { LocalizedDate } from "src/i18n/LocalizedDate";

interface Props {
  data: HistoryAllocatedBilling[] | undefined;
  loading?: boolean;

}

export const HistoryAllocatedTable: React.FC<Props> = ({ data, loading }) => {
  return (
    <Table dataSource={data} loading={loading} rowKey="id">
      <Table.Column title="StartTime" dataIndex="startTime"
        render={(time: string) => <LocalizedDate dateTimeString={time} />} />
      <Table.Column title="EndTime" dataIndex="endTime"
        render={(time: string) => <LocalizedDate dateTimeString={time} />} />
      <Table.Column title="Resources" dataIndex="resources"
        render={(resources: Resources) => <ResourcesModalLink resources={resources} />} />
      <Table.Column title="Amount" dataIndex="amount" />
      <Table.Column title="Payer" dataIndex="payerName" />
    </Table>
  )
}

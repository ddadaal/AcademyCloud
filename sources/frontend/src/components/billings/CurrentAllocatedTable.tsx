import React from "react";
import { Resources } from 'src/models/Resources';
import { Table } from 'antd';
import { Link } from "@reach/router";
import { Localized } from "src/i18n";
import { LocalizedDate } from "src/i18n/LocalizedDate";
import { ResourcesModalLink } from "src/components/resources/ResourcesModalLink";

interface AllocatedDataItem {
  subjectName: string;
  subjectLink: string;
  resources: Resources;
  amount: number;
  payerId: string;
  payerName: string;
}

interface Props {
  subjectType: "domain" | "project" | "user";
  data: AllocatedDataItem[] | undefined;
  loading?: boolean;
}

export const CurrentAllocatedTable: React.FC<Props> = ({ subjectType, data, loading }) => {
  return (
    <Table dataSource={data} loading={loading} rowKey="subjectName">
      <Table.Column title={subjectType} dataIndex="subject"
        render={(_, item: AllocatedDataItem) => <Link to={item.subjectLink}>{item.subjectName}</Link>} />
      <Table.Column title="Resources" dataIndex="resources"
        render={(resources: Resources) => <ResourcesModalLink resources={resources} />} />
      <Table.Column title="Should pay" dataIndex="amount" />
      <Table.Column title="Payer" dataIndex="payerName" />
      <Table.Column title="Next Due" dataIndex="nextDue" render={(date: string) => <LocalizedDate dateTimeString={date} />} />
    </Table>
  )
}

import React from "react";
import { lang, Localized } from "src/i18n";
import { Table } from "antd";
import { Flavor, InstanceStatus, Instance } from "src/models/Instance";
import { FlavorModalLink } from "src/components/flavor/FlavorModalLink";
import { LocalizedDate } from "src/i18n/LocalizedDate";

const root = lang.resources;


interface Props {
  data: Instance[] | undefined;
  loading: boolean;
}

export const InstanceTable: React.FC<Props> = ({ data, loading, children }) => {

  return (
    <Table dataSource={data} loading={loading} rowKey="id">
      <Table.Column title={<Localized id={root.instance.id} />} dataIndex="id" />
      <Table.Column title={<Localized id={root.instance.name} />} dataIndex="name" />
      <Table.Column title={<Localized id={root.instance.ip} />} dataIndex="ip" />
      <Table.Column title={<Localized id={root.instance.imageName} />} dataIndex="imageName" />
      <Table.Column title={<Localized id={root.instance.flavor} />} dataIndex="flavor"
        render={(flavor: Flavor) => <FlavorModalLink flavor={flavor} />} />
      <Table.Column title={<Localized id={root.instance.status.label} />} dataIndex="status"
        render={(status: InstanceStatus) => status in root.instance.status ? <Localized id={root.instance.status[status]} /> : status} />
      <Table.Column title={<Localized id={root.instance.vmState.label} />} dataIndex="vmState"
        render={(state: string) => state in root.instance.vmState ? <Localized id={root.instance.vmState[state]} /> : state} />
      <Table.Column title={<Localized id={root.instance.taskState.label} />} dataIndex="taskState"
        render={(state: string) => state in root.instance.taskState ? <Localized id={root.instance.taskState[state]} /> : state} />
      <Table.Column title={<Localized id={root.instance.powerState.label} />} dataIndex="powerState"
        render={(state: string) => state in root.instance.powerState ? <Localized id={root.instance.powerState[state]} /> : state} />
      <Table.Column title={<Localized id={root.instance.createTime} />} dataIndex="createTime"
        render={(createTime: string) => <LocalizedDate dateTimeString={createTime} />} />
      {children}
    </Table>
  )
};


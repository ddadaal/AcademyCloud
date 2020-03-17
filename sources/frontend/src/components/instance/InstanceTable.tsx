import React from "react";
import { lang, Localized } from "src/i18n";
import { getApiService } from "src/apis";
import { InstanceService } from "src/apis/resources/InstanceService";
import { Table } from "antd";
import { useAsync } from "react-async";
import { Flavor, InstanceStatus } from "src/models/Instance";
import { FlavorModalLink } from "src/components/flavor/FlavorModalLink";
import { LocalizedDate } from "src/i18n/LocalizedDate";

const root = lang.resources;

const service = getApiService(InstanceService);

const getInstances = () => service.getInstances().then(x => x.instances);

interface Props {
  refreshToken?: any;
}

export const InstanceTable: React.FC<Props> = ({ children, refreshToken }) => {

  const { data, isPending } = useAsync({ promiseFn: getInstances, watch: refreshToken });

  return (
    <Table dataSource={data} loading={isPending} rowKey="id">
      <Table.Column title={<Localized id={root.instance.id} />} dataIndex="id" />
      <Table.Column title={<Localized id={root.instance.name} />} dataIndex="name" />
      <Table.Column title={<Localized id={root.instance.ip} />} dataIndex="ip" />
      <Table.Column title={<Localized id={root.instance.imageName} />} dataIndex="imageName" />
      <Table.Column title={<Localized id={root.instance.flavor} />} dataIndex="flavor"
        render={(flavor: Flavor) => <FlavorModalLink flavor={flavor} />} />
      <Table.Column title={<Localized id={root.instance.status.label} />} dataIndex="status"
        render={(status: InstanceStatus) => <Localized id={root.instance.status[status]} />} />
      <Table.Column title={<Localized id={root.instance.createTime} />} dataIndex="createTime"
        render={(createTime: string) => <LocalizedDate dateTimeString={createTime} />} />
      {children}
    </Table>
  )
};


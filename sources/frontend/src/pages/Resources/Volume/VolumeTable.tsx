import React from "react";
import { lang, Localized } from "src/i18n";
import { getApiService } from "src/apis";
import { InstanceService } from "src/apis/resources/InstanceService";
import { Table } from "antd";
import { useAsync } from "react-async";
import { FlavorModalLink } from "src/components/flavor/FlavorModalLink";
import { LocalizedDate } from "src/i18n/LocalizedDate";
import { VolumeService } from "src/apis/resources/VolumeService";
import { Volume } from "src/models/Volume";
import { idString } from "src/utils/idString";

const root = lang.resources.volume;

const service = getApiService(VolumeService);

const getVolumes = () => service.getVolumes().then(x => x.volumes);


interface Props {
  refreshToken?: any;
}

export const VolumeTable: React.FC<Props> = (props) => {

  const { data, isPending } = useAsync({ promiseFn: getVolumes, watch: props.refreshToken });

  return (
    <Table dataSource={data} loading={isPending} rowKey="id">
      <Table.Column title={<Localized id={root.id} />} dataIndex="id" />
      <Table.Column title={<Localized id={root.size} />} dataIndex="size"
        render={(size: number) => `${size} GB`} />
      <Table.Column title={<Localized id={root.attachedToInstance} />} dataIndex="attachedToInstanceId"
        render={(_, volume: Volume) => `${volume.attachedToInstanceName} (${idString(volume.attachedToInstanceId)})`}
      />
      <Table.Column title={<Localized id={root.attachedToDevice} />} dataIndex="attachedToDevice" />
      <Table.Column title={<Localized id={root.createTime} />} dataIndex="createTime"
        render={(createTime: string) => <LocalizedDate dateTimeString={createTime} />} />
    </Table>
  )
};

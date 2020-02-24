import React from "react";
import { Resources } from "src/models/Resources";
import { Table } from "antd";
import { Localized, lang } from "src/i18n";

const { Column } = Table;

interface Props {
  resources: Resources;
}

const root = lang.components.resources;

export const ResourcesViewTable: React.FC<Props> = (props) => {

  return (
    <Table pagination={{ hideOnSinglePage: true }} dataSource={Object.entries(props.resources)}>
      <Column title={<Localized id={root.resourceType} />} dataIndex={0} key={0}
        render={(ty) => <Localized id={root[ty]} />} />
      <Column title={<Localized id={root.values} />} dataIndex={1} key={1} />
    </Table>
  );
}

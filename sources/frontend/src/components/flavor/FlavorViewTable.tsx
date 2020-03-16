import React from "react";
import { Table } from "antd";
import { Localized, lang } from "src/i18n";
import { Flavor } from "src/models/Instance";

const { Column } = Table;

interface Props {
  flavor: Flavor;
}

const root = lang.components.flavor;

export const FlavorViewTable: React.FC<Props> = (props) => {

  return (
    <Table pagination={{ hideOnSinglePage: true }} dataSource={Object.entries(props.flavor)} rowKey="0">
      <Column title={<Localized id={root.type} />} dataIndex={0}
        render={(ty) => <Localized id={root[ty]} />} />
      <Column title={<Localized id={root.value} />} dataIndex={1} />
    </Table>
  );
}

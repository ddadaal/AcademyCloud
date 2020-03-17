import React from "react";
import { InstanceTable } from 'src/components/instance/InstanceTable';
import { lang, Localized } from "src/i18n";
import { Table } from "antd";
import { Instance } from "src/models/Instance";

const root = lang.resources.instance;

interface Props {
  refreshToken?: any;
}

export const InstanceManagementTable: React.FC<Props> = ({ refreshToken }) => {
  return (
    <InstanceTable refreshToken={refreshToken}>
      <Table.Column title={<Localized id={root.actions.title} />}
        render={(_, instance: Instance) => (
          <div />
        )} />
    </InstanceTable>
  )
};

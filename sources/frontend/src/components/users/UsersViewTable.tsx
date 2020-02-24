import React from "react";
import { Table } from "antd";
import { LocalizedString, lang } from "src/i18n";
import { User } from "src/models/User";

const { Column } = Table;

interface Props {
  users: User[];
}

const root = lang.components.users;

export const UsersViewTable: React.FC<Props> = (props) => {

  return (
    <Table pagination={{ hideOnSinglePage: true }} dataSource={props.users}>
      <Column title={<LocalizedString id={root.id} />}
        dataIndex="id" key="id" />
      <Column title={<LocalizedString id={root.name} />}
        dataIndex="name" key="name" />
    </Table>
  )
}

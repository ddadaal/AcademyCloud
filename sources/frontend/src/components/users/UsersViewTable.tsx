import React from "react";
import { Table } from "antd";
import { Localized, lang } from "src/i18n";
import { User } from "src/models/User";

const { Column } = Table;

interface Props {
  users: User[];
}

const root = lang.components.users;

export const UsersViewTable: React.FC<Props> = (props) => {

  console.log(props.users);

  return (
    <Table pagination={{ hideOnSinglePage: true }} dataSource={props.users}>
      <Column title={<Localized id={root.id} />}
        dataIndex="id" key="id" />
      <Column title={<Localized id={root.name} />}
        dataIndex="name" key="name" />
      <Column title={<Localized id={root.active.title} />}
        dataIndex="active" key="id"
        render={(active: boolean) => (
          <Localized id={root.active[String(active)]}/>
        )}/>
    </Table>
  )
}

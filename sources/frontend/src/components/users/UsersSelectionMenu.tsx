import React from "react";
import { User } from "src/models/User";
import { lang, Localized } from "src/i18n";
import { useAsync } from "react-async";
import { Table } from "antd";

interface Props {
  selected: User[];
  getAllUsers: () => Promise<User[]>;
  onChange: (users: User[]) => void;
}

const root = lang.components.users;

export const UsersSelectionMenu: React.FC<Props> = ({ selected, getAllUsers, onChange }) => {

  const { data, isPending } = useAsync({ promiseFn: getAllUsers });

  console.log(data);

  return (
    <div>
      <Table rowSelection={{
        selectedRowKeys: selected.map((x) => x.id),
        onChange: (e) => onChange(data?.filter((x) => e.includes(x.id)) ?? []),
      }} dataSource={data} loading={isPending} rowKey="id">
        <Table.Column title={<Localized id={root.id}/>} dataIndex="id" />
        <Table.Column title={<Localized id={root.name}/>} dataIndex="name" />
        <Table.Column title={<Localized id={root.active.title}/>} dataIndex="active"
          render={(active: boolean) => <Localized id={root.active[String(active)]} />}
        />
      </Table>
    </div>
  );
}

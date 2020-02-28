import React, { useMemo, useState, useCallback } from "react";
import { User } from 'src/models/User';
import { UserRole } from "src/models/Scope";
import { lang, Localized } from "src/i18n";
import { Table, Popconfirm, notification, Tooltip, Divider } from "antd";
import { mergeAdminAndMember, UserWithRole } from "src/components/users/UserWithRole";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { RoleChangeSelect } from "src/components/users/UserRoleEditTable/RoleChangeSelect";
import { HttpError } from "src/apis/HttpService";
import { RemoveLink } from "src/components/users/UserRoleEditTable/RemoveLink";
import { DisabledA } from "src/components/DisabledA";
import { SetAsPayUserLink } from "src/components/users/UserRoleEditTable/SetAsPayUserLink";

const root = lang.components.users;

const opResult = lang.components.operationResult;

interface Props {

  admins: User[];
  members: User[];
  payUser: User;

  onRoleChange: (userId: string, role: UserRole) => Promise<void>;
  onPayUserSet: (user: User) => Promise<void>;
  onRemove: (user: User) => Promise<void>;
}

function useLoading(api: ReturnType<typeof useLocalizedNotification>[0], handler: (user: User) => Promise<void>, opName: string) {

  const [id, setId] = useState<string | undefined>(undefined);
  const handle = useCallback(async (user: User) => {
    try {
      api.info({ messageId: [opResult.inProgress, [opName]] });
      setId(id);
      await handler(user);
      api.success({ messageId: [opResult.success, [opName]] })
    } catch (e) {
      api.error({
        messageId: [opResult.fail, [opName]],
      });
    } finally {
      setId(undefined);
    }
  }, [handler]);

  return [id, handle] as const;
}


export const ExistingTable: React.FC<Props> = (props) => {
  const { admins, members, onRoleChange, onRemove, payUser, onPayUserSet } = props;

  const allUsers = useMemo(() => mergeAdminAndMember(admins, members), [admins, members]);

  const [api, contextHolder] = useLocalizedNotification();

  const [removingId, handleRemove] = useLoading(api, onRemove, root.remove.opName);

  const [settingPayUserId, handleSetPayUser] = useLoading(api, onPayUserSet, root.setAsPayUser.opName);

  return (
    <>
      {contextHolder}
      <Table dataSource={allUsers} rowKey="id">
        <Table.Column title={<Localized id={root.id} />} dataIndex="id" />
        <Table.Column title={<Localized id={root.name} />} dataIndex="name" />
        <Table.Column title={<Localized id={root.active.title} />}
          dataIndex="active" key="id"
          render={(active: boolean) => (
            <Localized id={root.active[String(active)]} />
          )} />
        <Table.Column title={<Localized id={root.payUser.title} />}
          dataIndex="role"
          render={(_, user: User) => (
            user.id === payUser.id ? <Localized id={root.payUser.yes} /> : null
          )} />
        <Table.Column title={<Localized id={root.role.title} />}
          dataIndex="role"
          render={(role: UserRole, user: User) => (
            <RoleChangeSelect
              disabled={!!settingPayUserId || removingId === user.id || user.id === payUser.id}
              user={user}
              initialRole={role}
              onChange={onRoleChange}
            />
          )} />
        <Table.Column title={<Localized id={root.actions} />}
          dataIndex="role"
          render={(_, user: User) => (
            user.id === payUser.id
              ? (
                <span>
                  <DisabledA disabled={true} message={<Localized id={root.remove.errors.payUser} />}>
                    <Localized id={root.remove.link} />
                  </DisabledA>
                  <Divider type="vertical" />
                  <DisabledA disabled={true} message={<Localized id={root.remove.errors.payUser} />}>
                    <Localized id={root.setAsPayUser.link} />
                  </DisabledA>
                </span>
              )
              : (
                <span>
                  <RemoveLink user={user} onRemove={handleRemove} disabled={!!settingPayUserId || removingId === user.id} />
                  <Divider type="vertical" />
                  <SetAsPayUserLink user={user} onSet={handleSetPayUser} disabled={!!settingPayUserId || removingId === user.id} />
                </span>
              )

          )} />
      </Table>
    </>
  )
}

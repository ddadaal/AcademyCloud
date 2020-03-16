import React, { useMemo, useState, useCallback } from "react";
import { User } from 'src/models/User';
import { UserRole } from "src/models/Scope";
import { lang, Localized } from "src/i18n";
import { Table, Divider } from "antd";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { RoleChangeSelect } from "src/components/users/UserRoleEditTable/RoleChangeSelect";
import { RemoveLink } from "src/components/users/UserRoleEditTable/RemoveLink";
import { DisabledA } from "src/components/DisabledA";
import { SetAsPayUserLink } from "src/components/users/UserRoleEditTable/SetAsPayUserLink";
import { Resources } from "src/models/Resources";
import { FullUser, mergeInformation } from "../FullUser";
import { SetUserResourcesLink } from "src/components/users/UserRoleEditTable/SetUserResources";
import { ResourcesModalLink } from "src/components/resources/ResourcesModalLink";

const root = lang.components.users;

const opResult = lang.components.operationResult;

interface Props {

  admins: User[];
  members: User[];
  payUser: User;
  userResources?: { [userId: string]: Resources };

  onResourcesChange?: (user: User, resources: Resources) => Promise<void>;
  onRoleChange: (user: User, role: UserRole) => Promise<void>;
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
  const { admins, members, userResources, onResourcesChange, onRoleChange, onRemove, payUser, onPayUserSet } = props;

  const allUsers = useMemo(() => mergeInformation(admins, members, userResources), [admins, members, userResources]);

  const [api, contextHolder] = useLocalizedNotification();

  const [removingId, handleRemove] = useLoading(api, onRemove, root.remove.opName);

  const [settingPayUserId, handleSetPayUser] = useLoading(api, onPayUserSet, root.setAsPayUser.opName);

  return (
    <>
      {contextHolder}
      <Table dataSource={allUsers} rowKey="id">
        <Table.Column title={<Localized id={root.id} />} dataIndex="id" />
        <Table.Column title={<Localized id={root.name} />} dataIndex="name" />
        <Table.Column title={<Localized id={root.payUser.title} />}
          dataIndex="role"
          render={(_, user: FullUser) => (
            user.id === payUser.id ? <Localized id={root.payUser.yes} /> : null
          )} />
        {
          onResourcesChange
            ? (
              <Table.Column title={<Localized id={root.quota} />}
                dataIndex="resources"
                render={(resources: Resources) => <ResourcesModalLink resources={resources} />} />
            ) : null}
        <Table.Column title={<Localized id={root.role.title} />}
          dataIndex="role"
          render={(role: UserRole, user: FullUser) => (
            <RoleChangeSelect
              disabled={!!settingPayUserId || removingId === user.id || user.id === payUser.id}
              user={user}
              role={role}
              onChange={onRoleChange}
            />
          )} />
        <Table.Column title={<Localized id={root.actions} />}
          dataIndex="role"
          render={(_, user: FullUser) => (
            user.id === payUser.id
              ? (
                <span>
                  <DisabledA disabled={true} message={<Localized id={root.remove.errors.payUser} />}>
                    <Localized id={root.remove.link} />
                  </DisabledA>
                  <Divider type="vertical" />
                  <DisabledA disabled={true} >
                    <Localized id={root.setAsPayUser.link} />
                  </DisabledA>
                  <SetUserResourcesLink user={user} onConfirm={onResourcesChange} />
                </span>
              )
              : (
                <span>
                  <RemoveLink user={user} onRemove={handleRemove} disabled={!!settingPayUserId || removingId === user.id} />
                  <Divider type="vertical" />
                  <SetAsPayUserLink user={user} onSet={handleSetPayUser} disabled={!!settingPayUserId || removingId === user.id} />
                  <SetUserResourcesLink user={user} onConfirm={onResourcesChange} />
                </span>
              )

          )} />
      </Table>
    </>
  )
}

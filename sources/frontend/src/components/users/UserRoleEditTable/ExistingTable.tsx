import React, { useMemo, useState, useCallback } from "react";
import { User } from 'src/models/User';
import { UserRole } from "src/models/Scope";
import { lang, Localized } from "src/i18n";
import { Table, Popconfirm, notification } from "antd";
import { mergeAdminAndMember, UserWithRole } from "src/components/users/UserWithRole";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { RoleChangeSelect } from "src/components/users/UserRoleEditTable/RoleChangeSelect";
import { HttpError } from "src/apis/HttpService";

const root = lang.components.users;

const opResult = lang.components.operationResult;

const removeInProgressKey = "removeInProgress";

interface Props {

  allUsers: UserWithRole[];

  onRoleChange: (userId: string, role: UserRole) => Promise<void>;
  onRemove: (userId: string) => Promise<void>;
}

const RemoveLink: React.FC<{
  user: User;
  onRemove: (userId: string) => void;
  removing: boolean;
}> = ({ user, onRemove, removing }) => {

  return (
    <Popconfirm
      disabled={removing}
      title={<Localized id={root.remove.prompt} />}
      onConfirm={() => onRemove(user.id)}>
      <a>
        <Localized id={root.remove.link} />
      </a>
    </Popconfirm>
  );
}


export const ExistingTable: React.FC<Props> = (props) => {
  const { allUsers, onRoleChange, onRemove } = props;

  const [removingId, setRemovingId] = useState<string | undefined>(undefined);

  const [api, contextHolder] = useLocalizedNotification();

  const handleRemove = useCallback(async (id: string) => {
    try {
      api.info({ key: removeInProgressKey, messageId: [opResult.inProgress, [root.remove.opName]] });
      setRemovingId(id);
      await onRemove(id);
      notification.close(removeInProgressKey);
      api.success({ messageId: [opResult.success, [root.remove.opName]] })
    } catch (e) {
      api.error({
        messageId: [opResult.fail, [root.remove.opName]],
        descriptionId: root.remove.errors[e.code],
      });
    } finally {
      setRemovingId(undefined);
    }
  }, [onRemove]);

  return (
    <>
      {contextHolder}
      <Table dataSource={allUsers} rowKey="id">
        <Table.Column title={<Localized id={root.id} />} dataIndex="id" />
        <Table.Column title={<Localized id={root.name} />} dataIndex="name" />
        <Table.Column title={<Localized id={root.role.title} />}
          dataIndex="role"
          render={(role: UserRole, user: User) => (
            <RoleChangeSelect disabled={removingId === user.id} user={user} initialRole={role} onChange={onRoleChange} />
          )} />
        <Table.Column title={<Localized id={root.actions} />}
          dataIndex="role"
          render={(_, user: User) => (
            <span>
              <RemoveLink user={user} onRemove={handleRemove} removing={removingId === user.id} />
            </span>
          )} />
      </Table>
    </>
  )
}

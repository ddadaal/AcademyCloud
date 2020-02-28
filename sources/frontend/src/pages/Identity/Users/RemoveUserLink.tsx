import React, { useCallback } from "react";
import { Scope, isSystemScope } from "src/models/Scope";
import { User } from "src/models/User";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { Localized, lang } from "src/i18n";
import { ExclamationOutlined } from "@ant-design/icons";
import { getApiService } from "src/apis";
import { DomainsService } from "src/apis/identity/DomainsService";
import { UsersService } from "src/apis/identity/UsersService";
import { Modal } from "antd";

const root = lang.identity.users.remove;
const opResult = lang.components.operationResult;

interface Props {
  isSystem: boolean;
  domainId: string;
  user: User;
  reload: () => void;
}

const usersService = getApiService(UsersService);
const domainsService = getApiService(DomainsService);

export const RemoveUserLink: React.FC<Props> = ({ isSystem, domainId, user, reload }) => {

  const [api, contextHolder] = useLocalizedNotification();

  const [modalApi, modalContextHolder] = Modal.useModal();

  const removeUser = useCallback(async ([userId]: [string]) => {
    if (isSystem) {
      await usersService.removeUserFromSystem(userId);
    } else {
      await domainsService.removeUser(domainId, userId);
    }
  }, [isSystem, domainId])

  const onClick = useCallback(() => {
    modalApi.confirm({
      title: <Localized id={root.confirmPrompt} replacements={[user.id]} />,
      icon: <ExclamationOutlined />,
      onOk: () => removeUser([user.id])
        .then(() => {
          reload();
          api.success({
            messageId: [opResult.success, [root.opName]],
          })
        })
        .catch(({ code }) => api.error({
          messageId: [opResult.fail, [root.opName]],
        })),
    });
  }, [user]);

  return (
    <span>
      {contextHolder}
      {modalContextHolder}
      <a onClick={onClick}>
        <Localized id={root.link} />
      </a>
    </span>
  );
}

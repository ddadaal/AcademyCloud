import React, { useCallback } from "react";
import { UserDomainAssignment } from "src/models/UserDomainAssignment";
import { getApiService } from "src/apis";
import { AccountService } from "src/apis/identity/AccountService";
import { Localized, lang } from "src/i18n";
import { Modal } from "antd";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { ExclamationOutlined } from "@ant-design/icons";
import { AvailableScopesStore } from "src/stores/AvailableScopesStore";
import { useStore } from "simstate";
import { DisabledA } from "src/components/DisabledA";

interface Props {
  domain: UserDomainAssignment;
  reload: () => void;
  disabled: boolean;
}

const root = lang.identity.account.joinedDomains.table;
const opResult = lang.components.operationResult;

const service = getApiService(AccountService);

const exitDomain = async ([domainId]: [string]) => {
  await service.exitDomain(domainId);
};

export const ExitDomainLink: React.FC<Props> = ({ reload, domain, disabled }) => {

  const [api, contextHolder] = useLocalizedNotification();

  const [modalApi, modalContextHolder] = Modal.useModal();

  const availableScopesStore = useStore(AvailableScopesStore);

  const onClick = useCallback(() => {
    modalApi.confirm({
      title: <Localized id={root.confirmExit} replacements={[domain.domainName]} />,
      icon: <ExclamationOutlined />,
      onOk: () => exitDomain([domain.domainId])
        .then(() => {
          reload();
          api.success({
            messageId: [opResult.success, [root.opName]],
          });
          availableScopesStore.updateScopes();
        })
        .catch(({ data }) => api.error({
          messageId: [opResult.fail, [root.opName]],
          descriptionId: root.error[data.reason]
        })),
      okText: <Localized id={root.okText} />,
      cancelText: <Localized id={root.cancelText} />,
    });
  }, [domain]);

  return (
    <span>
      {contextHolder}
      {modalContextHolder}
      <DisabledA disabled={disabled} message={<Localized id={root.error.cannotExitCurrentDomain} />} onClick={onClick}>
        <Localized id={root.exit} />
      </DisabledA>
    </span>
  );
}

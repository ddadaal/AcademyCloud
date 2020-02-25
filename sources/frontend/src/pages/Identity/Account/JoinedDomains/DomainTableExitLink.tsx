import React, { useCallback } from "react";
import { UserDomainAssignment } from "src/models/UserDomainAssignment";
import { getApiService } from "src/apis";
import { PersonalAccountService } from "src/apis/identity/PersonalAccountService";
import { Localized, lang } from "src/i18n";
import { Modal } from "antd";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { ExclamationOutlined } from "@ant-design/icons";

interface Props {
  domain: UserDomainAssignment;
  reload: () => void;
}

const root = lang.identity.account.joinedDomains.table;
const opResult = lang.components.operationResult;

const service = getApiService(PersonalAccountService);

const exitDomain = async ([domainId]) => {
  await service.exitDomain(domainId);
};

export const DomainTableExitLink: React.FC<Props> = ({ reload, domain }) => {

  const [api, contextHolder] = useLocalizedNotification();

  const [modalApi, modalContextHolder] = Modal.useModal();

  const onClick = useCallback(() => {
    modalApi.confirm({
      title: <Localized id={root.confirmExit} replacements={[domain.domainName]} />,
      icon: <ExclamationOutlined />,
      onOk: () => exitDomain([domain.domainId])
        .then(() => {
          reload();
          api.success({
            messageId: [opResult.success, [root.opName]],
          })
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
      <a onClick={onClick}>
        <Localized id={root.exit} />
      </a>
    </span>
  );
}

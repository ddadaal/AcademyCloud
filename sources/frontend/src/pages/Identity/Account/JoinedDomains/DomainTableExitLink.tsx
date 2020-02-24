import React, { useCallback, useState } from "react";
import { UserDomainAssignment } from "src/models/UserDomainAssignment";
import { useAsync } from "react-async";
import { getApiService } from "src/apis";
import { PersonalAccountService, ExitDomainsError } from "src/apis/identity/PersonalAccountService";
import { LocalizedString, lang } from "src/i18n";
import { Modal } from "antd";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { ExclamationOutlined } from "@ant-design/icons";

interface Props {
  domain: UserDomainAssignment;
  reload: () => void;
}

const root = lang.identity.account.joinedDomains.table;

const service = getApiService(PersonalAccountService);

const exitDomain = async ([domainId]) => {
  await service.exitDomain(domainId);
};

export const DomainTableExitLink: React.FC<Props> = ({ reload, domain }) => {

  const [api, contextHolder] = useLocalizedNotification();

  const [modalApi, modalContextHolder] = Modal.useModal();

  const onClick = useCallback(() => {
    modalApi.confirm({
      title: <LocalizedString id={root.confirmExit} replacements={[domain.domainName]} />,
      icon: <ExclamationOutlined />,
      onOk: () => exitDomain([domain.domainId])
        .then(() => {
          reload();
          api.success({
            messageId: root.success,
          })
        })
        .catch(({ data }) => api.error({
          messageId: root.error.title,
          descriptionId: root.error[data.reason]
        })),
      okText: <LocalizedString id={root.okText} />,
      cancelText: <LocalizedString id={root.cancelText} />,
    });
  }, [domain]);

  return (
    <span>
      {contextHolder}
      {modalContextHolder}
      <a onClick={onClick}>
        <LocalizedString id={root.exit} />
      </a>
    </span>
  );
}

import React, { useCallback } from "react";
import { lang, Localized } from "src/i18n";
import { Project } from "src/models/Project";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { Modal, Popover, Tooltip } from "antd";
import { ExclamationOutlined } from "@ant-design/icons";
import { getApiService } from "src/apis";
import { Domain } from "src/models/Domain";
import { DomainsService } from "src/apis/identity/DomainsService";

interface Props {
  domain: Domain;
  reload: () => void;
}

const root = lang.identity.domains.delete;
const opResult = lang.components.operationResult;

const service = getApiService(DomainsService);
const deleteDomain = ([domainId]: [string]) => {
  return service.deleteDomain(domainId);
}

export const DeleteDomainLink: React.FC<Props> = ({ domain, reload }) => {
  const [api, contextHolder] = useLocalizedNotification();

  const [modalApi, modalContextHolder] = Modal.useModal();

  const onClick = useCallback(() => {
    modalApi.confirm({
      title: <Localized id={root.confirmPrompt} replacements={[domain.name]} />,
      icon: <ExclamationOutlined />,
      onOk: () => deleteDomain([domain.name])
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
  }, [domain, reload]);

  return (
    <span>
      {contextHolder}
      {modalContextHolder}
      {domain.active
        ? (
          <a onClick={onClick}>
            <Localized id={root.link} />
          </a>
        )
        : (
          <Tooltip title={<Localized id={root.inactive} />}>
            <span>
              <Localized id={root.link} />
            </span>
          </Tooltip>
        )
      }
    </span>
  );
}

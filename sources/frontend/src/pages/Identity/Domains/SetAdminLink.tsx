import React, { useState } from "react";
import { Domain } from "src/models/Domain";
import { Localized, lang } from "src/i18n";
import { getApiService } from "src/apis";
import { DomainsService } from "src/apis/identity/DomainsService";
import { useAsync } from "react-async";
import Modal from "antd/lib/modal/Modal";
import { ResourcesEditForm } from "src/components/resources/ResourcesEditForm";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";

interface Props {
  domain: Domain;
  reload: () => void;
}
const root = lang.identity.domains.setAdmins;

const service = getApiService(DomainsService);

const updateAdmins = async ([domainId, adminIds]) => {
  await service.setAdmins(domainId, adminIds);
}

export const SetAdminLink: React.FC<Props> = ({ domain, reload }) => {

  const [modalShown, setModalShown] = useState(false);

  const [resources, setResources] = useState(domain.resources);

  const [api, contextHolder] = useLocalizedNotification();

  const { run, isPending } = useAsync({
    deferFn: updateAdmins,
    onResolve: () => {
      reload();
      setModalShown(false);
      api.success({ messageId: root.success });
    },
    onReject: () => {
      api.error({ messageId: root.failed });
    }
  });

  return (
    <>
      <a onClick={() => setModalShown(true)}>
        <Localized id={root.title} />
      </a>
      {contextHolder}
      <Modal
        visible={modalShown}
        title={<Localized id={root.title} />}
        onOk={() => run(domain.id, resources)}
        onCancel={() => setModalShown(false)}
        confirmLoading={isPending}
      >
        <ResourcesEditForm initial={resources} onValuesChange={setResources} />
      </Modal>
    </>
  );
}

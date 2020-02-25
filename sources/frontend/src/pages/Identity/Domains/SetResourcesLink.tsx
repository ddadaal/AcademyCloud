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
const root = lang.identity.domains;
const opResult = lang.components.operationResult;

const service = getApiService(DomainsService);

const updateResources = async ([domainId, resources]) => {
  await service.setResources(domainId, resources);
}

export const SetResourcesLink: React.FC<Props> = ({ domain, reload }) => {

  const [modalShown, setModalShown] = useState(false);

  const [resources, setResources] = useState(domain.resources);

  const [api, contextHolder] = useLocalizedNotification();

  const { run, isPending } = useAsync({
    deferFn: updateResources,
    onResolve: () => {
      reload();
      setModalShown(false);
      api.success({ messageId: [opResult.success, [root.setAdmins.title]] });
    },
    onReject: () => {
      api.error({ messageId: [opResult.fail, [root.setAdmins.title]] });
    }
  });

  return (
    <>
      <a onClick={() => setModalShown(true)}>
        <Localized id={root.setResources.title} />
      </a>
      {contextHolder}
      <Modal
        visible={modalShown}
        title={<Localized id={root.setResources.title} />}
        onOk={() => run(domain.id, resources)}
        onCancel={() => setModalShown(false)}
        confirmLoading={isPending}
      >
        <ResourcesEditForm initial={resources} onValuesChange={setResources} />
      </Modal>
    </>
  );
}

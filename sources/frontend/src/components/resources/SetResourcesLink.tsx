import React, { useState, useCallback } from "react";
import { Localized, lang } from "src/i18n";
import { useAsync } from "react-async";
import Modal from "antd/lib/modal/Modal";
import { ResourcesEditForm } from "src/components/resources/ResourcesEditForm";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { Resources } from "src/models/Resources";

interface Props {
  initial: Resources;
  onConfirm: (resources: Resources) => Promise<void>;
}
const root = lang.components.resources.setResources;
const opResult = lang.components.operationResult;

export const SetResourcesLink: React.FC<Props> = ({ initial, onConfirm }) => {

  const [modalShown, setModalShown] = useState(false);

  const [resources, setResources] = useState(initial);

  const [api, contextHolder] = useLocalizedNotification();

  const deferFn = useCallback(async ([resources]: [Resources]) => {
    await onConfirm(resources);
  }, [onConfirm])

  const { run, isPending } = useAsync({
    deferFn,
    onResolve: () => {
      setModalShown(false);
      api.success({ messageId: [opResult.success, [root.title]] });
    },
    onReject: () => {
      api.error({ messageId: [opResult.fail, [root.title]] });
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
        onOk={() => run(resources)}
        onCancel={() => setModalShown(false)}
        confirmLoading={isPending}
      >
        <ResourcesEditForm initial={resources} onValuesChange={setResources} />
      </Modal>
    </>
  );
}

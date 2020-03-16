import React, { useState, useCallback } from "react";
import { Localized, lang } from "src/i18n";
import { useAsync } from "react-async";
import { Modal, Form } from "antd";
import { ResourcesEditForm } from "src/components/resources/ResourcesEditForm";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { Resources } from "src/models/Resources";

interface Props {
  initial: Resources;
  onConfirm: (resources: Resources) => Promise<void>;
  getAvailableQuota: () => Promise<{ used: Resources; total: Resources }>;
}
const root = lang.components.resources.setResources;
const opResult = lang.components.operationResult;

export const SetResourcesLink: React.FC<Props> = ({ initial, onConfirm, getAvailableQuota }) => {

  const [form] = Form.useForm();

  const [modalShown, setModalShown] = useState(false);

  const [api, contextHolder] = useLocalizedNotification();

  const deferFn = useCallback(async () => {
    const resources = await form.validateFields() as Resources;
    await onConfirm(resources);
  }, [onConfirm])


  const { run, isPending } = useAsync({
    deferFn,
    onResolve: () => {
      setModalShown(false);
      api.success({ messageId: [opResult.success, [root.title]] });
    },
    onReject: (e) => {
      // if errorFields in e, it is a error caused by form validation, ignore it.
      if (!("errorFields" in e)) {
        api.error({ messageId: [opResult.fail, [root.title]] });
      }
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
        onOk={run}
        onCancel={() => setModalShown(false)}
        confirmLoading={isPending}
        destroyOnClose={true}
      >
        <ResourcesEditForm
          getAvailableQuota={getAvailableQuota}
          initial={initial}
          form={form} />
      </Modal>
    </>
  );
}

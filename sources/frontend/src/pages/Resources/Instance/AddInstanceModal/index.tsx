import React, { useCallback, useState } from "react";
import { Modal, Form, Input, Divider, Spin } from "antd";
import { Localized, lang } from "src/i18n";
import { required } from "src/utils/validateMessages";
import { FlavorSelect } from "src/pages/Resources/Instance/AddInstanceModal/FlavorSelect";
import { ResourcesService } from "src/apis/resources/ResourcesService";
import { getApiService } from "src/apis";
import { useAsync } from "react-async";
import { minus, Resources } from "src/models/Resources";
import { StrongLabel } from "src/components/StrongLabel";
import { ImageSelect } from "src/pages/Resources/Instance/AddInstanceModal/ImageSelect";
import { InstanceService } from "src/apis/resources/InstanceService";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";

const root = lang.resources.instance.add;

const opResult = lang.components.operationResult;

interface Props {
  visible: boolean;
  close: () => void;
  onCreated: () => void;
}

const service = getApiService(InstanceService);
const createInstance = async (data: { name: string; flavor: string; image: string; volume: string }) => {
  await service.createInstance(data.name, data.flavor, data.image, parseInt(data.volume));
}


export const AddInstanceModal: React.FC<Props> = ({ visible, close, onCreated }) => {

  const [submitting, setSubmitting] = useState(false);

  const [api, contextHolder] = useLocalizedNotification();

  const [form] = Form.useForm();

  const onOk = useCallback(async () => {
    const data = await form.validateFields();
    setSubmitting(true);
    try {
      await createInstance(data as any);
      api.success({ messageId: [opResult.success, [root.opName]] });
      onCreated();
      close();
    } catch (e) {
      api.error({ messageId: [opResult.fail, [root.opName]] });
    } finally {
      setSubmitting(false);
    }
  }, [form]);


  return (
    <Modal
      visible={visible}
      title={<Localized id={root.button} />}
      onOk={onOk}
      onCancel={close}
      confirmLoading={submitting}
    >
      {contextHolder}
      <Form form={form} layout="vertical" initialValues={{ name: new Date().toISOString() }}>
        <Form.Item label={<StrongLabel id={root.name} />} name="name" rules={[{ required: true, message: required }]}>
          <Input />
        </Form.Item>
        <FlavorSelect />
        <ImageSelect />
      </Form>

    </Modal >
  )
};

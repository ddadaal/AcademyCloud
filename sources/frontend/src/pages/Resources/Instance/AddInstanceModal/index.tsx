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

const root = lang.resources.instance.add;

interface Props {
  visible: boolean;
  close: () => void;
}

const service = getApiService(InstanceService);
const createInstance = async (data: { name: string; flavor: string; image: string; volume: number }) => {
  await service.createInstance(data.name, data.flavor, data.image, data.volume);
}


export const AddInstanceModal: React.FC<Props> = ({ visible, close }) => {

  const [submitting, setSubmitting] = useState(false);

  const [form] = Form.useForm();

  const onOk = useCallback(async () => {
    const data = await form.validateFields();
    setSubmitting(true);
    try {
      await createInstance(data as any);
    } catch (e) {

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
      <Form form={form} layout="vertical" initialValues={{ name: Math.random() }}>
        <Form.Item label={<StrongLabel id={root.name} />} name="name" rules={[{ required: true, message: required }]}>
          <Input />
        </Form.Item>
        <FlavorSelect />
        <ImageSelect />
      </Form>

    </Modal >
  )
};

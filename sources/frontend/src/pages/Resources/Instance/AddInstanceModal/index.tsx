import React from "react";
import { Modal, Form, Input, Divider, Spin } from "antd";
import { Localized, lang } from "src/i18n";
import { required } from "src/utils/validateMessages";
import { FlavorSelect } from "src/pages/Resources/Instance/AddInstanceModal/FlavorSelect";
import { ResourcesService } from "src/apis/resources/ResourcesService";
import { getApiService } from "src/apis";
import { useAsync } from "react-async";
import { minus, Resources } from "src/models/Resources";
import { StrongLabel } from "src/components/StrongLabel";

const root = lang.resources.instance.add;

interface Props {
  visible: boolean;
  close: () => void;
}



export const AddInstanceModal: React.FC<Props> = ({ visible, close }) => {

  const [form] = Form.useForm();


  return (
    <Modal
      visible={visible}
      title={<Localized id={root.button} />}
      onCancel={close}
    >
      <Form form={form} layout="vertical">
        <Form.Item label={<StrongLabel id={root.name} />} name="name" rules={[{ required: true, message: required }]}>
          <Input />
        </Form.Item>
        <FlavorSelect />
      </Form>

    </Modal >
  )
};

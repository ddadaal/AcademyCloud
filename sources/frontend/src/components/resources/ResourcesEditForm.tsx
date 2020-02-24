import React from "react";
import { Form, Input } from "antd";
import { Resources } from "src/models/Resources";
import { lang, Localized } from "src/i18n";

interface Props {
  initial: Resources;
  onValuesChange: (resources: Resources) => void;
}

const root = lang.components.resources;

export const ResourcesEditForm: React.FC<Props> = ({ initial, onValuesChange }) => {

  const [form] = Form.useForm();

  return (
    <Form
      onValuesChange={(_, allValues) => onValuesChange(allValues as any)}
      form={form}
      initialValues={initial}
    >
      <Form.Item label={<Localized id={root.cpu} />} name="cpu">
        <Input />
      </Form.Item>
      <Form.Item label={<Localized id={root.memory} />} name="memory">
        <Input />
      </Form.Item>
      <Form.Item label={<Localized id={root.storage} />} name="storage">
        <Input />
      </Form.Item>
    </Form>
  );
}

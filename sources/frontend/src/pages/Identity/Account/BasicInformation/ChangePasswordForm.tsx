import React from "react";
import { Form, Input, Button, Spin, notification } from "antd";
import { getApiService } from "src/apis";
import { PersonalAccountService } from "src/apis/identity/PersonalAccountService";
import { useAsync } from "react-async";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { lang, Localized } from "src/i18n";


const service = getApiService(PersonalAccountService);

const updatePassword = async ([original, updated]) => {
  return await service.updatePassword(original, updated);
};

const root = lang.identity.account.basic.changePassword;

export const ChangePasswordForm: React.FC = () => {

  const [form] = Form.useForm();

  const [api, contextHolder] = useLocalizedNotification();

  const { isPending, run } = useAsync({
    onResolve: () => {
      form.setFieldsValue({ original: "", updated: "" });
      api.success({ messageId: root.success });
    },
    onReject: () => {
      api.error({ messageId: root.failed, descriptionId: root.failedDescription });
    },
    deferFn: updatePassword
  });

  //123

  const handleSubmit = ({ original, updated }) => {
    run(original, updated);
  };

  return (
    <Spin spinning={isPending}>
      {contextHolder}
      <Form initialValues={undefined} layout="vertical" form={form} onFinish={handleSubmit}>
        <Form.Item
          rules={[{ required: true }]}
          label={<Localized id={root.original} />}
          name="original"
        >
          <Input type="password" />
        </Form.Item>
        <Form.Item
          rules={[{ required: true }]}
          label={<Localized id={root.newPassword} />}
          name="updated"
        >
          <Input type="password" />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" >
            <Localized id={root.update} />
          </Button>
        </Form.Item>
      </Form>
    </Spin>
  )
}

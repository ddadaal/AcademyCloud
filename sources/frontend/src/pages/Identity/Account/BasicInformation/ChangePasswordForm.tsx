import React from "react";
import { Form, Input, Button, Spin } from "antd";
import { getApiService } from "src/apis";
import { AccountService } from "src/apis/identity/AccountService";
import { useAsync } from "react-async";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { lang, Localized } from "src/i18n";
import { required } from "src/utils/validateMessages";


const service = getApiService(AccountService);

const updatePassword = async ([original, updated]) => {
  return await service.updatePassword(original, updated);
};

const root = lang.identity.account.basic.changePassword;

const opResult = lang.components.operationResult;

export const ChangePasswordForm: React.FC = () => {

  const [form] = Form.useForm();

  const [api, contextHolder] = useLocalizedNotification();

  const { isPending, run } = useAsync({
    onResolve: () => {
      form.setFieldsValue({ original: "", updated: "" });
      api.success({ messageId: [opResult.success, [root.opName]] });
    },
    onReject: () => {
      api.error({ messageId: [opResult.fail, [root.opName]], descriptionId: root.failedDescription });
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
          rules={[{ required: true, message: required }]}
          label={<Localized id={root.original} />}
          name="original"
        >
          <Input type="password" />
        </Form.Item>
        <Form.Item
          rules={[{ required: true, message: required }]}
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

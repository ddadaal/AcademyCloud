import React from "react";
import { Form, Input, Button, Spin } from "antd";
import { getApiService } from "src/apis";
import { AccountService } from "src/apis/identity/AccountService";
import { useAsync } from "react-async";
import { lang, Localized } from "src/i18n";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { email, required } from "src/utils/validateMessages";


const service = getApiService(AccountService);

const getProfile = async () => {
  const { profile } = await service.getProfile();
  return profile;
}

const updateProfile = async ([email]) => {
  const resp = await service.updateProfile({ email });
  return resp.profile;
};

const root = lang.identity.account.basic.profile;

const opResult = lang.components.operationResult;

export const ProfileForm: React.FC = () => {

  const [form] = Form.useForm();

  const [api, contextHolder] = useLocalizedNotification();

  const { isPending } = useAsync({
    promiseFn: getProfile,
    onResolve: (profile) => {
      form.setFieldsValue(profile);
    },
  });

  const { isPending: updatePending, run, } = useAsync({
    deferFn: updateProfile,
    onResolve: (profile) => {
      form.setFieldsValue(profile);
      api.success({ messageId: [opResult.success, [root.opName]] });
    },
  });


  const handleSubmit = ({ email }) => {
    run(email);
  };

  return (
    <Spin spinning={isPending || updatePending}>
      {contextHolder}
      <Form initialValues={undefined} layout="vertical" form={form} onFinish={handleSubmit}>
        <Form.Item label={<Localized id={root.id} />} name="id">
          <Input disabled={true} />
        </Form.Item>
        <Form.Item label={<Localized id={root.username} />} name="username">
          <Input disabled={true} />
        </Form.Item>
        <Form.Item
          rules={[{ type: "email", message: email }, { required: true, message: required }]}
          label={<Localized id={root.email} />}
          name="email"
        >
          <Input />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" >
            <Localized id={root.update} />
          </Button>
        </Form.Item>
      </Form>
    </Spin >
  )
}

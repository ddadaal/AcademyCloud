import React, { useCallback, useState } from "react";
import { Form, Input, Button, Spin } from "antd";
import { Profile } from "src/models/Profile";
import { getApiService } from "src/apis";
import { PersonalAccountService, ProfileResponse } from "src/apis/identity/PersonalAccountService";
import { useAsync } from "react-async";
import { lang, LocalizedString } from "src/i18n";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";


const service = getApiService(PersonalAccountService);

const getProfile = async () => {
  const { profile } = await service.getProfile();
  return profile;
}

const updateProfile = async ([email]) => {
  const resp = await service.updateProfile({ email });
  return resp.profile;
};

const root = lang.identity.account.basic.profile;

export const ProfileForm: React.FC = () => {

  const [form] = Form.useForm();

  const [api, contextHolder] = useLocalizedNotification();

  const { isPending, run } = useAsync({
    promiseFn: getProfile,
    onResolve: (profile) => {
      form.setFieldsValue(profile);
      api.success({ messageId: root.success });
    },
    deferFn: updateProfile
  });


  const handleSubmit = ({ email }) => {
    run(email);
  };

  return (
    <Spin spinning={isPending}>
      <Form initialValues={undefined} layout="vertical" form={form} onFinish={handleSubmit}>
        <Form.Item label={<LocalizedString id={root.id} />} name="id">
          <Input disabled={true} />
        </Form.Item>
        <Form.Item label={<LocalizedString id={root.username} />} name="username">
          <Input disabled={true} />
        </Form.Item>
        <Form.Item
          rules={[{ type: "email", required: true }]}
          label={<LocalizedString id={root.email} />}
          name="email"
        >
          <Input />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" >
            <LocalizedString id={root.update} />
          </Button>
        </Form.Item>
      </Form>
    </Spin >
  )
}

import React, { useCallback, useState } from "react";
import { Form, Input, Button, Spin } from "antd";
import { Profile } from "src/models/Profile";
import { getApiService } from "src/apis";
import { PersonalAccountService, ProfileResponse } from "src/apis/identity/PersonalAccountService";
import { useAsync } from "react-async";


const service = getApiService(PersonalAccountService);

const getProfile = async () => {
  const { profile } = await service.getProfile();
  return profile;
}

const updateProfile = async ([email]) => {
  const resp = await service.updateProfile({ email });
  return resp.profile;
};

export const ProfileForm: React.FC = () => {

  const [form] = Form.useForm();

  const { isPending, run } = useAsync({
    promiseFn: getProfile,
    onResolve: (profile) => form.setFieldsValue(profile),
    deferFn: updateProfile
  });


  const handleSubmit = ({ email }) => {
    run(email);
  };

  return (
    <Spin spinning={isPending}>
      <Form initialValues={undefined} layout="vertical" form={form} onFinish={handleSubmit}>
        <Form.Item label="User Id" name="id">
          <Input disabled={true} />
        </Form.Item>
        <Form.Item label="Username" name="username">
          <Input disabled={true} />
        </Form.Item>
        <Form.Item label="Email" name="email">
          <Input />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" >
            Change
          </Button>
        </Form.Item>
      </Form>
    </Spin>
  )
}

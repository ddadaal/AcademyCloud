import React from "react";
import { Form, Input, Button, Spin, notification } from "antd";
import { getApiService } from "src/apis";
import { PersonalAccountService } from "src/apis/identity/PersonalAccountService";
import { useAsync } from "react-async";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";


const service = getApiService(PersonalAccountService);

const updatePassword = async ([password]) => {
  return await service.updatePassword(password);
};

export const ChangePasswordForm: React.FC = () => {

  const [form] = Form.useForm();

  const { isPending, run } = useAsync({
    onResolve: () => {
      form.setFieldsValue({ password: "" });
    },
    deferFn: updatePassword
  });


  const handleSubmit = ({ password }) => {
    run(password);
  };

  return (
    <Spin spinning={isPending}>
      <Form initialValues={undefined} layout="vertical" form={form} onFinish={handleSubmit}>
        <Form.Item label={"Input new password"} name="password">
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

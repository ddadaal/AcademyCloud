import React, { useCallback, useState } from "react";
import { RouteComponentProps, Link, navigate } from "@reach/router";
import { Form, Input, Button } from "antd";
import { UserOutlined, LockOutlined, MailOutlined } from "@ant-design/icons";
import { FormButton } from "src/pages/Home/HomePageLayout";

const { Item } = Form;

export const RegisterForm: React.FC<RouteComponentProps> = () => {

  const [registering, setRegistering] = useState(false);

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const onFinish = useCallback(async (values: { [key: string]: any }) => {
    const { username, password, email } = values;

    try {
      setRegistering(true);
    } finally {
      setRegistering(false);
    }

  }, []);

  return (
    <Form onFinish={onFinish} name="register">
      <Item name="username" rules={[{ required: true, message: "Please insert username." }]}>
        <Input disabled={registering} prefix={<UserOutlined className="site-form-item-icon" />} placeholder="Username" />
      </Item>
      <Item name="password" rules={[{ required: true, message: "Please input password" }]}>
        <Input type="password" disabled={registering} prefix={<LockOutlined className="site-form-item-icon" />} placeholder="Password" />
      </Item>
      <Item name="email" rules={[
        { type: "email", message: "Please input email." },
        { required: true, message: "Please input password" }
      ]}>
        <Input prefix={<MailOutlined className="site-form-item-icon" />} placeholder="Email" />
      </Item>
      <Item>
        <FormButton loading={registering} type="primary" htmlType="submit" className="login-form-button">
          Register
        </FormButton>
      </Item>
    </Form>
  );
}


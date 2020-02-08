import { Form, Input, Checkbox, Button } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import React from "react";
import { lang, useMultiLocalized, LocalizedString } from "src/i18n";
import styled from "styled-components";

interface LoginInfo {
  domain: string;
  username: string;
  password: string;
}

const root = lang.homepage.loginForm;

export function LoginForm() {

  const onFinish = (values: { [key: string]: string }) => {
    console.log('Received values of form: ', values);
  };

  const localizedStrings = useMultiLocalized(
    root.school,
    root.username,
    root.password,
    root.remember,
    root.forget,
    root.login,
  ) as string[];

  return (
    <Form
      name="normal_login"
      className="login-form"
      initialValues={{ remember: true }}
      onFinish={onFinish}
    >
      <Form.Item
        name="domain"
        rules={[{ required: true, message: <LocalizedString id={root.schoolPrompt} /> }]}
      >
        <Input prefix={<UserOutlined className="site-form-item-icon" />} placeholder={localizedStrings[0]} />
      </Form.Item>
      <Form.Item
        name="username"
        rules={[{ required: true, message: <LocalizedString id={root.usernamePrompt} /> }]}
      >
        <Input prefix={<UserOutlined className="site-form-item-icon" />} placeholder={localizedStrings[1]} />
      </Form.Item>
      <Form.Item
        name="password"
        rules={[{ required: true, message: <LocalizedString id={root.passwordPrompt} /> }]}
      >
        <Input
          prefix={<LockOutlined className="site-form-item-icon" />}
          type="password"
          placeholder={localizedStrings[2]}
        />
      </Form.Item>
      <Form.Item>
        <Form.Item name="remember" valuePropName="checked" noStyle>
          <Checkbox>{localizedStrings[3]}</Checkbox>
        </Form.Item>

        <ForgotLink>
          {localizedStrings[4]}
        </ForgotLink>
      </Form.Item>
      <Form.Item>
        <LoginFormButton type="primary" htmlType="submit">
          {localizedStrings[5]}
        </LoginFormButton>
      </Form.Item>
    </Form >
  );
}

const LoginFormButton = styled(Button)`
  width: 100%;
`;

const ForgotLink = styled.a`
  float: right;
`;

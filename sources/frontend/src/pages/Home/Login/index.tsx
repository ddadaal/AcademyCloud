import { Form, Input, Checkbox, notification } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import React, { useState } from "react";
import { lang, useMultiLocalized, LocalizedString, I18nStore } from "src/i18n";
import styled from "styled-components";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { getApiService } from "src/apis";
import { AccountService } from "src/apis/account/AccountService";
import { navigate, RouteComponentProps } from "@reach/router";
import { FormButton } from "src/pages/Home/HomePageLayout";

const root = lang.homepage.loginForm;

export const LoginForm: React.FC<RouteComponentProps> = () => {

  const userStore = useStore(UserStore);
  const i18nStore = useStore(I18nStore);

  const [loggingIn, setLoggingIn] = useState(false);

  // The original signature is any.
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const onFinish = async (values: { [key: string]: any }) => {
    const { username, password } = values;
    try {
      setLoggingIn(true);
      const accountService = getApiService(AccountService);
      const targets = await accountService.getScopes(username, password);
      if (targets.length === 0) {
        notification.error({ message: i18nStore.translate(root.loginFailTitle), description: i18nStore.translate(root.noScope) });
        return;
      }

      console.log("Available scopes", targets);

      // randomly select a target to login for now
      const target = targets[0];
      const loginResponse = await accountService.login(username, password, target);
      userStore.login({ username, token: loginResponse.token }, values.remember);

      console.log("Login success.");

      navigate("/resources");
    } catch (e) {
      notification.error({ message: i18nStore.translate(root.loginFailTitle), description: i18nStore.translate(root.other) });
    } finally {
      setLoggingIn(false);
    }
  };

  const localizedStrings = useMultiLocalized(
    root.school,
    root.username,
    root.password,
    root.remember,
    root.forget,
    root.login,
    root.register,
  ) as string[];

  return (
    <Form
      name="normal_login"
      className="login-form"
      initialValues={{ remember: true }}
      onFinish={onFinish}

    >
      <Form.Item
        name="username"
        rules={[{ required: true, message: <LocalizedString id={root.usernamePrompt} /> }]}
      >
        <Input disabled={loggingIn} prefix={<UserOutlined className="site-form-item-icon" />} placeholder={localizedStrings[1]} />
      </Form.Item>
      <Form.Item
        name="password"
        rules={[{ required: true, message: <LocalizedString id={root.passwordPrompt} /> }]}
      >
        <Input
          prefix={<LockOutlined className="site-form-item-icon" />}
          type="password"
          disabled={loggingIn}
          placeholder={localizedStrings[2]}
        />
      </Form.Item>
      <Form.Item>
        <Form.Item name="remember" valuePropName="checked" noStyle>
          <Checkbox disabled={loggingIn}>{localizedStrings[3]}</Checkbox>
        </Form.Item>

        <ForgotLink>
          {localizedStrings[4]}
        </ForgotLink>
      </Form.Item>
      <Form.Item>
        <FormButton loading={loggingIn} type="primary" htmlType="submit">
          {localizedStrings[5]}
        </FormButton>
      </Form.Item>
    </Form >
  );
}


const ForgotLink = styled.a`
  float: right;
`;

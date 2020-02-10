import { Form, Input, Checkbox, Button, notification } from 'antd';
import { UserOutlined, LockOutlined, HomeOutlined } from '@ant-design/icons';
import React, { useState } from "react";
import { lang, useMultiLocalized, LocalizedString, I18nStore } from "src/i18n";
import styled from "styled-components";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { getApiService } from "src/apis";
import { AccountService } from "src/apis/account/AccountService";
import { navigate } from "@reach/router";

const root = lang.homepage.loginForm;

export function LoginForm() {

  const userStore = useStore(UserStore);
  const i18nStore = useStore(I18nStore);

  const [loggingIn, setLoggingIn] = useState(false);

  // The original signature is any.
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const onFinish = async (values: { [key: string]: any }) => {
    const { username, password, domain } = values;
    try {
      setLoggingIn(true);
      const accountService = getApiService(AccountService);
      const targets = await accountService.getScopeableTargets(username, password, domain);
      if (targets.length === 0) {
        notification.error({ message: i18nStore.translate(root.loginFailTitle), description: i18nStore.translate(root.noScope)});
        return;
      }

      console.log("Available scopes", targets);

      // randomly select a target to login for now
      const target = targets[0];
      const loginResponse = await accountService.login(username, password, domain, target.type === "project" ? target.name : undefined);
      userStore.login({ username, token: loginResponse.token, scope: loginResponse.scope}, values.remember);

      console.log("Login success.");

      navigate("/resources");
    } catch (e) {
      notification.error({ message: i18nStore.translate(root.loginFailTitle), description: i18nStore.translate(root.other)});
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
        <Input disabled={loggingIn} prefix={<HomeOutlined className="site-form-item-icon" />} placeholder={localizedStrings[0]} />
      </Form.Item>
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
        <LoginFormButton loading={loggingIn} type="primary" htmlType="submit">
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
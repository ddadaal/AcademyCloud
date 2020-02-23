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
import { PageMetadata } from "src/utils/PageMetadata";

const root = lang.homepage.loginForm;

export const LoginForm: React.FC<RouteComponentProps> = () => {

  const userStore = useStore(UserStore);

  const [api, contextHolder] = notification.useNotification();

  const [loggingIn, setLoggingIn] = useState(false);

  // The original signature is any.
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const onFinish = async (values: { [key: string]: any }) => {
    const { username, password } = values;
    try {
      setLoggingIn(true);
      const accountService = getApiService(AccountService);
      const scopesResp = await accountService.getScopes(username, password);

      // select default; if not, select lastLogin; if not, the first
      const scope = scopesResp.defaultScope ?? scopesResp.lastLoginScope ?? scopesResp.scopes[0];
      const loginResponse = await accountService.login(username, password, scope);
      userStore.login({
        username,
        scope,
        availableScopes: scopesResp.scopes,
        token: loginResponse.token,
        remember: values.remember,
      });

      await navigate("/resources");

    } catch (e) {
      api.error({
        message: <LocalizedString id={root.loginFailTitle} />,
        description: <LocalizedString id={root.other} />,
      });
    } finally {
      setLoggingIn(false);
    }
  };

  const [username, password] = useMultiLocalized(
    root.username,
    root.password,
  ) as string[];

  return (
    <Form
      name="normal_login"
      className="login-form"
      initialValues={{ remember: true }}
      onFinish={onFinish}
    >
      {contextHolder}
      <PageMetadata titleId={root.title} />
      <Form.Item
        name="username"
        rules={[{ required: true, message: <LocalizedString id={root.usernamePrompt} /> }]}
      >
        <Input
          disabled={loggingIn}
          prefix={<UserOutlined className="site-form-item-icon" />}
          placeholder={username} />
      </Form.Item>
      <Form.Item
        name="password"
        rules={[{ required: true, message: <LocalizedString id={root.passwordPrompt} /> }]}
      >
        <Input
          prefix={<LockOutlined className="site-form-item-icon" />}
          type="password"
          disabled={loggingIn}
          placeholder={password}
        />
      </Form.Item>
      <Form.Item>
        <Form.Item name="remember" valuePropName="checked" noStyle>
          <Checkbox disabled={loggingIn}><LocalizedString id={root.remember} /></Checkbox>
        </Form.Item>

        <ForgotLink>
          <LocalizedString id={root.forget} />
        </ForgotLink>
      </Form.Item>
      <Form.Item>
        <FormButton loading={loggingIn} type="primary" htmlType="submit">
          <LocalizedString id={root.login} />
        </FormButton>
      </Form.Item>
    </Form >
  );
}


const ForgotLink = styled.a`
  float: right;
`;

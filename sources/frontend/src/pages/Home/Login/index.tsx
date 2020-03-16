import { Form, Input, Checkbox, notification } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import React, { useState } from "react";
import { lang, useMultiLocalized, Localized } from "src/i18n";
import styled from "styled-components";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { getApiService } from "src/apis";
import { AuthenticationService } from "src/apis/account/AuthenticationService";
import { navigate, RouteComponentProps } from "@reach/router";
import { FormButton } from "src/pages/Home/HomePageLayout";
import { PageMetadata } from "src/utils/PageMetadata";
import { required } from "src/utils/validateMessages";
import { AvailableScopesStore } from "src/stores/AvailableScopesStore";
import { isResourcesDisabled } from 'src/models/Scope';

const root = lang.homepage.loginForm;

export const LoginForm: React.FC<RouteComponentProps> = () => {

  const userStore = useStore(UserStore);
  const availableScopesStore = useStore(AvailableScopesStore);

  const [api, contextHolder] = notification.useNotification();

  const [loggingIn, setLoggingIn] = useState(false);

  // The original signature is any.
  const onFinish = async (values: { [key: string]: any }) => {
    const { username, password } = values;
    try {
      setLoggingIn(true);
      const accountService = getApiService(AuthenticationService);
      const scopesResp = await accountService.getScopes(username, password);

      // select default; if not, select lastLogin; if not, the first
      const scope = scopesResp.defaultScope ?? scopesResp.lastLoginScope ?? scopesResp.scopes[0];
      const loginResponse = await accountService.login(username, password, scope);
      userStore.login({
        userId: loginResponse.userId,
        username,
        scope,
        token: loginResponse.token,
        remember: values.remember,
      });
      availableScopesStore.setScopes(scopesResp.scopes, values.remember);
      await navigate(isResourcesDisabled(scope) ? "/expenses" : "/resources");

    } catch (e) {
      api.error({
        message: <Localized id={root.loginFailTitle} />,
        description: <Localized id={root.other} />,
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
        rules={[{ required: true, message: required }]}
      >
        <Input
          disabled={loggingIn}
          prefix={<UserOutlined className="site-form-item-icon" />}
          placeholder={username} />
      </Form.Item>
      <Form.Item
        name="password"
        rules={[{ required: true, message: required }]}
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
          <Checkbox disabled={loggingIn}><Localized id={root.remember} /></Checkbox>
        </Form.Item>

        <ForgotLink>
          <Localized id={root.forget} />
        </ForgotLink>
      </Form.Item>
      <Form.Item>
        <FormButton loading={loggingIn} type="primary" htmlType="submit">
          <Localized id={root.login} />
        </FormButton>
      </Form.Item>
    </Form >
  );
}


const ForgotLink = styled.a`
  float: right;
`;

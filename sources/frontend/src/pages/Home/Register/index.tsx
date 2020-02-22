import React, { useCallback, useState } from "react";
import { RouteComponentProps, navigate } from "@reach/router";
import { Form, Input, notification } from "antd";
import { UserOutlined, LockOutlined, MailOutlined } from "@ant-design/icons";
import { FormButton } from "src/pages/Home/HomePageLayout";
import { lang, LocalizedString, useMultiLocalized, I18nStore } from "src/i18n";
import { AccountService } from "src/apis/account/AccountService";
import { getApiService } from "src/apis";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { HttpError } from "src/apis/HttpService";
import { PageMetadata } from "src/utils/PageMetadata";

const { Item } = Form;

const root = lang.homepage.registerForm;

export const RegisterForm: React.FC<RouteComponentProps> = () => {

  const userStore = useStore(UserStore);
  const i18nStore = useStore(I18nStore);
  const [registering, setRegistering] = useState(false);

  // the origin signature is any
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const onFinish = useCallback(async (values: { [key: string]: any }) => {
    const { username, password } = values;
    setRegistering(true);

    const api = getApiService(AccountService);

    try {
      const registeringResponse = await api.register(username, password);
      userStore.login(
        username,
        registeringResponse.scope,
        [registeringResponse.scope],
        registeringResponse.token,
        true
      );
      await navigate("/resources");
    } catch (e) {
      const ex = e as HttpError;
      if (ex.status === 403) {
        // conflict
        notification.error({
          message: i18nStore.translate(root.registerFailed),
          description: i18nStore.translate(root.conflict)
        });
      } else {
        notification.error({
          message: i18nStore.translate(root.registerFailed),
          description: i18nStore.translate(root.other)
        });
      }
    } finally {
      setRegistering(false);
    }

  }, []);

  const [username, password, email] = useMultiLocalized(root.username, root.password, root.email) as string[];

  return (
    <Form onFinish={onFinish} name="register">
      <PageMetadata titleId={root.title} />
      <Item name="username"
        rules={[{ required: true, message: <LocalizedString id={root.usernamePrompt} /> }]}>
        <Input disabled={registering} prefix={<UserOutlined className="site-form-item-icon" />} placeholder={username} />
      </Item>
      <Item name="password" rules={[{ required: true, message: <LocalizedString id={root.passwordPrompt} /> }]}>
        <Input type="password" disabled={registering} prefix={<LockOutlined className="site-form-item-icon" />} placeholder={password} />
      </Item>
      <Item name="email" rules={[
        { type: "email", message: <LocalizedString id={root.emailPrompt} /> },
        { required: true, message: <LocalizedString id={root.emailPrompt} /> }
      ]}>
        <Input prefix={<MailOutlined className="site-form-item-icon" />} placeholder={email} />
      </Item>
      <Item>
        <FormButton loading={registering} type="primary" htmlType="submit" className="login-form-button">
          <LocalizedString id={root.register} />
        </FormButton>
      </Item>
    </Form>
  );
}


import React, { useState } from "react";
import { useStore } from 'simstate';
import { UserStore } from 'src/stores/UserStore';
import { Menu, Dropdown, notification } from "antd";
import { DownOutlined, BookOutlined } from "@ant-design/icons";
import { lang, Localized, I18nStore } from "src/i18n";
import { scopeId, scopeName, Scope, isSystemScope } from "src/models/Scope";
import { ClickableA } from "src/components/ClickableA";
import { getApiService } from "src/apis";
import { AccountService } from "src/apis/account/AccountService";

const root = lang.nav.scopeIndicator;

export const ScopeIndicator: React.FC = () => {

  const [changingTo, setChangingTo] = useState<Scope | null>(null);

  const userStore = useStore(UserStore);
  const { user } = userStore;

  const i18nStore = useStore(I18nStore);

  const [api, contextHolder] = notification.useNotification();

  if (!user) { return null; }

  const { scope: currentScope, availableScopes } = user;

  if (isSystemScope(currentScope)) {
    return (
      <ClickableA>
        <BookOutlined /> <Localized id={root.system} />
      </ClickableA>
    )
  }

  const projectScopes = availableScopes.filter((x) => x.projectId)
  const domainScopes = availableScopes.filter((x) => !x.projectId)

  const menuItems = [] as React.ReactNode[];

  const scopeNameWithRole = (scope: Scope) => (
    <>
      {scopeName(scope)}
      {scope.role === "admin" ? <> (<Localized id={root.admin} />)</> : null}
    </>
  );

  const onChange = (scope: Scope) => async () => {
    // re-login
    const apiService = getApiService(AccountService);
    setChangingTo(scope);
    try {
      const resp = await apiService.changeScope(scope);
      userStore.login({ ...user, token: resp.token, scope });

      api.success({
        message: <Localized id={root.success} />,
      });
    } catch (e) {
      api.error({
        message: <Localized id={root.fail} />,
      });
    } finally {
      setChangingTo(null);
    }
  };

  if (projectScopes.length > 0) {
    menuItems.push(
      <Menu.Item key="projectPrompt" disabled={true}>
        <Localized id={root.projects} />
      </Menu.Item>
    );
    menuItems.push(...projectScopes.map((x) => (
      <Menu.Item disabled={!!changingTo} onClick={onChange(x)} key={scopeId(x)}>
        {scopeNameWithRole(x)}
      </Menu.Item>
    )));
  }

  if (domainScopes.length > 0) {

    menuItems.push(
      <Menu.Divider />,
      <Menu.Item key="domainPrompt" disabled={true}>
        <Localized id={root.domains} />
      </Menu.Item>
    );
    menuItems.push(...domainScopes.map((x) => (
      <Menu.Item disabled={!!changingTo} onClick={onChange(x)} key={scopeId(x)}>
        {scopeNameWithRole(x)}
      </Menu.Item>
    )));
  }

  const menu = (
    <Menu selectedKeys={[scopeId(currentScope)]}>
      {menuItems}
    </Menu>
  );

  return (
    <Dropdown overlay={menu}>
      <ClickableA>
        {contextHolder}
        <BookOutlined />
        {changingTo
          ? <Localized id={root.changing} replacements={[scopeNameWithRole(changingTo)]} />
          : scopeNameWithRole(currentScope)}
        <DownOutlined />
      </ClickableA>
    </Dropdown>
  )


}

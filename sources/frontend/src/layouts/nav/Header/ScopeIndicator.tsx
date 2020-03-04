import React, { useState, Fragment } from "react";
import { useStore } from 'simstate';
import { UserStore } from 'src/stores/UserStore';
import { Menu, Dropdown } from "antd";
import { DownOutlined, BookOutlined } from "@ant-design/icons";
import { lang, Localized } from "src/i18n";
import { scopeId, scopeName, Scope, isSystemScope, isSocialScope, isAdmin } from "src/models/Scope";
import { ClickableA } from "src/components/ClickableA";
import { getApiService } from "src/apis";
import { AuthenticationService } from "src/apis/account/AuthenticationService";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { AvailableScopesStore } from "src/stores/AvailableScopesStore";

const root = lang.nav.scopeIndicator;

const opResult = lang.components.operationResult;

export const ScopeIndicator: React.FC = () => {

  const [changingTo, setChangingTo] = useState<Scope | null>(null);

  const userStore = useStore(UserStore);
  const availableScopesStore = useStore(AvailableScopesStore);

  const { user } = userStore;

  const [api, contextHolder] = useLocalizedNotification();

  if (!user) { return null; }

  const { scope: currentScope } = user;
  const { scopes, reloading } = availableScopesStore;

  if (reloading) {
    return (
      <ClickableA>
        <BookOutlined/> <Localized id={root.reloading} />
      </ClickableA>
    );
  }

  if (isSystemScope(currentScope)) {
    return (
      <ClickableA>
        <BookOutlined /> <Localized id={root.system} />
      </ClickableA>
    );
  }

  const socialScope = scopes.find(isSocialScope);
  const projectScopes = scopes.filter((x) => x.projectId && !x.social)
  const domainScopes = scopes.filter((x) => !x.projectId)

  const menuItems = [] as React.ReactNode[];

  const scopeNameWithRole = (scope: Scope) => (
    <Fragment key={scopeId(scope)}>
      {
        isSocialScope(scope)
          ? <Localized id={root.social} />
          : (
            <>
              {scopeName(scope)}
              {isAdmin(scope) ? <> (<Localized id={root.admin} />)</> : null}
            </>
          )
      }
    </Fragment>
  );

  const onChange = (scope: Scope) => async () => {
    // re-login
    const apiService = getApiService(AuthenticationService);
    setChangingTo(scope);
    try {
      const resp = await apiService.changeScope(scope);
      userStore.login({ ...user, token: resp.token, scope });
      api.success({
        messageId: [opResult.success, [root.opName]],
      });
    } catch (e) {
      api.error({
        messageId: [opResult.fail, [root.opName]],
        descriptionId: root.failDescription,
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
    if (menuItems.length > 0) {
      menuItems.push(<Menu.Divider key="domainDivider" />);
    }
    menuItems.push(
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

  // add social scope
  if (socialScope) {
    if (menuItems.length > 0) {
      menuItems.push(<Menu.Divider key="socialDivider" />);
    }
    menuItems.push(
      <Menu.Item disabled={!!changingTo} onClick={onChange(socialScope)} key={scopeId(socialScope)}>
        {scopeNameWithRole(socialScope)}
      </Menu.Item>
    );
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
  );


}

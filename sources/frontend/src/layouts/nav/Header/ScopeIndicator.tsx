import React from "react";
import { useStore } from 'simstate';
import { UserStore } from 'src/stores/UserStore';
import { Scope } from "src/models/account";
import { Menu, Dropdown, Button } from "antd";
import { DownOutlined, BookOutlined } from "@ant-design/icons";
import { ClickableA } from "src/utils/ClickableA";
import { lang, LocalizedString } from "src/i18n";
import { ScopeClass } from "src/models/Scope";

const root = lang.nav.scopeIndicator;

export const ScopeIndicator: React.FC = () => {
  const userStore = useStore(UserStore);
  const { user } = userStore;

  if (!user) { return null; }

  const currentScope = new ScopeClass(user.scope);
  const availableScopes = user.availableScopes.map((s) => new ScopeClass(s));

  const projectScope = availableScopes.filter((x) => x.projectId)
  const domainScope = availableScopes.filter((x) => !x.projectId)

  const menuItems = [] as React.ReactNode[];

  const onChange = (scope: Scope) => () => {
    userStore.changeScope(scope);
  };

  if (projectScope.length > 0) {
    menuItems.push(
      <Menu.Item key="projectPrompt" disabled={true}>
        <LocalizedString id={root.projects} />
      </Menu.Item>
    );
    menuItems.push(...projectScope.map((x) => (
      <Menu.Item onClick={onChange(x)} key={x.scopeId}>
        {x.scopeName}
      </Menu.Item>
    )));
  }

  if (domainScope.length > 0) {

    menuItems.push(
      <Menu.Divider />,
      <Menu.Item key="domainPrompt" disabled={true}>
        <LocalizedString id={root.domains} />
      </Menu.Item>
    );
    menuItems.push(...domainScope.map((x) => (
      <Menu.Item onClick={onChange(x)} key={x.scopeId}>
        {x.scopeName}
      </Menu.Item>
    )));
  }

  const menu = (
    <Menu selectedKeys={[currentScope.scopeId]}>
      {menuItems}
    </Menu>
  );

  return (
    <Dropdown overlay={menu}>
      <ClickableA>
        <BookOutlined />{currentScope.scopeName} <DownOutlined />
      </ClickableA>
    </Dropdown>
  )


}

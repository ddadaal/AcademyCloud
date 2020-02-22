import React from "react";
import { useStore } from 'simstate';
import { UserStore } from 'src/stores/UserStore';
import { Menu, Dropdown } from "antd";
import { DownOutlined, BookOutlined } from "@ant-design/icons";
import { ClickableA } from "src/utils/ClickableA";
import { lang, LocalizedString } from "src/i18n";
import { scopeId, scopeName, Scope } from "src/models/Scope";

const root = lang.nav.scopeIndicator;

export const ScopeIndicator: React.FC = () => {
  const userStore = useStore(UserStore);
  const { user } = userStore;

  if (!user) { return null; }


  const { scope: currentScope, availableScopes } = user;

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
      <Menu.Item onClick={onChange(x)} key={scopeId(x)}>
        {scopeName(x)}
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
      <Menu.Item onClick={onChange(x)} key={scopeId(x)}>
        {scopeName(x)}
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
        <BookOutlined />{scopeName(currentScope)} <DownOutlined />
      </ClickableA>
    </Dropdown>
  )


}

import React from "react";
import { useStore } from 'simstate';
import { UserStore } from 'src/stores/UserStore';
import { Scope } from "src/models/account";
import { Menu, Dropdown, Button } from "antd";
import { DownOutlined, BookOutlined } from "@ant-design/icons";
import { ClickableA } from "src/utils/ClickableA";

function scopeId(scope: Scope): string {
  return `${scope.domainId}+${scope.projectId ?? ""}`;
}

function scopeName(scope: Scope): string {
  if (scope.projectName) {
    return `${scope.domainName} - ${scope.projectName}`;
  } else {
    return scope.domainName;
  }
}

export const ScopeIndicator: React.FC = () => {
  const userStore = useStore(UserStore);

  const { scope, availableScopes } = userStore.user;

  const projectScope = availableScopes.filter((x) => x.projectId)
  const domainScope = availableScopes.filter((x) => !x.projectId)

  const menuItems = [] as React.ReactNode[];

  const onChange = (scope: Scope) => (e: unknown) => {
    userStore.changeScope(scope);
  };

  if (projectScope.length > 0) {
    menuItems.push(
      <Menu.Item key="projectPrompt" disabled={true}>
        Projects
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
        Domains
      </Menu.Item>
    );
    menuItems.push(...domainScope.map((x) => (
      <Menu.Item onClick={onChange(x)} key={scopeId(x)}>
        {scopeName(x)}
      </Menu.Item>
    )));
  }

  const menu = (
    <Menu selectedKeys={[scopeId(scope)]}>
      {menuItems}
    </Menu>
  );

  return (
    <Dropdown overlay={menu}>
      <ClickableA>
        <BookOutlined />{scopeName(scope)} <DownOutlined />
      </ClickableA>
    </Dropdown>
  )


}

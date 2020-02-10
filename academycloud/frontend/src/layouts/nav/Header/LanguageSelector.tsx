import React from "react"
import { Dropdown, Menu, Button } from 'antd';
import { DownOutlined, GlobalOutlined } from "@ant-design/icons"
import { useStore } from "simstate";
import { I18nStore } from "src/i18n";
import { allLanguages } from "src/i18n/definition";
import styled from "styled-components";

export function LanguageSelector() {
  const i18nStore = useStore(I18nStore);

  const { currentLanguage, setLanguage } = i18nStore;

  const menus = React.useMemo(() =>
    allLanguages
      .map((x) =>
        <Menu.Item key={x.metadata.id}>
          <a onClick={() => setLanguage(x)}>
            {x.metadata.name}
          </a>
        </Menu.Item >
      ), [currentLanguage]);

  return (
    <Dropdown  overlay={<Menu selectedKeys={[currentLanguage.metadata.id]}>{menus}</Menu>}>
      <a className="ant-dropdown-link">
        <GlobalOutlined style={{ margin: 0}} />
      </a>
    </Dropdown>
  )
}

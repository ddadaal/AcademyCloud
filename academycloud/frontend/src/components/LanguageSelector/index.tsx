import React from "react"
import { Dropdown, Menu, Button } from 'antd';
import { DownOutlined } from "@ant-design/icons"
import { useStore } from "simstate";
import { I18nStore } from "src/i18n";
import { allLanguages } from "src/i18n/definition";

export function LanguageSelector() {
  const i18nStore = useStore(I18nStore);

  const { currentLanguage, setLanguage } = i18nStore;

  const menus = allLanguages
    .filter((x) => x.metadata.id !== currentLanguage.metadata.id)
    .map((x) =>
      <Menu.Item key={x.metadata.id}>
        <a onClick={() => setLanguage(x)}>
          {x.metadata.name}
        </a>
      </Menu.Item >
    );

  return (
    <Dropdown overlay={<Menu>{menus}</Menu>}>
      <Button>
        {currentLanguage.metadata.name}
        <DownOutlined />
      </Button>
    </Dropdown>
  )
}

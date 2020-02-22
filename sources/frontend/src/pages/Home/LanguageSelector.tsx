import React, { useCallback } from "react";
import { Radio, Dropdown, Menu } from "antd";
import { I18nStore } from "src/i18n";
import { useStore } from "simstate";
import { allLanguages } from "src/i18n/definition";
import { RadioChangeEvent } from "antd/lib/radio";
import { DownOutlined, GlobalOutlined } from '@ant-design/icons';
import styled from "styled-components";
import { ClickableA } from "src/utils/ClickableA";

export function HomepageLanguageSelector() {

  const { currentLanguage, changeLanguage } = useStore(I18nStore);

  return (
    <Dropdown trigger={["click", "hover"]} overlay={(
      <Menu selectedKeys={[currentLanguage.metadata.id]}>
        {allLanguages
          .filter((x) => x.metadata.id !== currentLanguage.metadata.id)
          .map((x) => (
            <Menu.Item key={x.metadata.id}>
              <ClickableA onClick={() => changeLanguage(x.metadata.id)}>
                {x.metadata.name}
              </ClickableA>
            </Menu.Item>
          ))
        }
      </Menu>)}>
      <ClickableA >
        <GlobalOutlined />   {currentLanguage.metadata.name} <DownOutlined />
      </ClickableA>
    </Dropdown>
  );

}


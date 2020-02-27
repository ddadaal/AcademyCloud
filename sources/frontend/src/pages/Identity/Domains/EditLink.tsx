import React from "react";
import { Dropdown, Menu } from "antd";
import { DownOutlined } from "@ant-design/icons";
import { lang, Localized } from "src/i18n";
import { SetAdminLink } from "src/pages/Identity/Domains/SetAdminLink";
import { Domain } from "src/models/Domain";
import { SetResourcesLink } from "src/pages/Identity/Domains/SetResourcesLink";
import { SetPayUserLink } from "src/pages/Identity/Domains/SetPayUserLink";

const root = lang.identity.domains;

interface Props {
  domain: Domain;
  reload: () => void;
}

export const EditLink: React.FC<Props> = ({ domain, reload }) => {
  return (
    <Dropdown overlay={(
      <Menu>
        <Menu.Item>
          <SetPayUserLink domain={domain} reload={reload} />
        </Menu.Item>
        <Menu.Item>
          <SetAdminLink domain={domain} reload={reload} />
        </Menu.Item>
        <Menu.Item>
          <SetResourcesLink domain={domain} reload={reload} />
        </Menu.Item>
      </Menu>
    )}>
      <a>
        <Localized id={root.edit} /> <DownOutlined />
      </a>
    </Dropdown>
  )
}

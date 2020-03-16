import React from "react";
import { Dropdown, Menu } from "antd";
import { DownOutlined } from "@ant-design/icons";
import { lang, Localized } from "src/i18n";
import { Project } from "src/models/Project";
import { ManageUsersLink } from "src/pages/Identity/Projects/ManageUsersLink";
import { SetProjectQuotasLink } from "src/pages/Identity/Projects/SetProjectQuotaLink";

const root = lang.identity.projects.table;

interface Props {
  domainId: string;
  project: Project;
  reload: () => void;
}

export const EditLink: React.FC<Props> = ({ project, reload, domainId }) => {
  return (
    <Dropdown overlay={(
      <Menu>
        <Menu.Item>
          <ManageUsersLink project={project} reload={reload} />
        </Menu.Item>
        <Menu.Item>
          <SetProjectQuotasLink domainId={domainId} project={project} reload={reload} />
        </Menu.Item>
      </Menu>
    )}>
      <a>
        <Localized id={root.edit} /> <DownOutlined />
      </a>
    </Dropdown>
  )
}

import React, { useCallback, useState, useRef } from "react";
import { InstanceTable } from 'src/components/instance/InstanceTable';
import { lang, Localized } from "src/i18n";
import { Table, Dropdown, Menu } from "antd";
import { Instance } from "src/models/Instance";
import { ClickableA } from "src/components/ClickableA";
import { DownOutlined } from "@ant-design/icons";
import { getApiService } from "src/apis";
import { InstanceService } from "src/apis/resources/InstanceService";
import { NotificationInstance } from "antd/lib/notification";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { useInterval } from "src/utils/useInterval";

const root = lang.resources.instance.actions;
const opResult = lang.components.operationResult;

interface Props {
  refreshToken?: any;
  reload: () => void;
}

const action = (id: string, onClick: () => Promise<void>, reload: () => void, api: ReturnType<typeof useLocalizedNotification>[0]) => (
  <Menu.Item onClick={async () => {
    api.info({ messageId: [opResult.inProgress, [id]] });
    await onClick();
    api.success({ messageId: [opResult.success, [id]] });
    reload();
  }}>
    <ClickableA><Localized id={id} /></ClickableA>
  </Menu.Item >
)

const service = getApiService(InstanceService);

export const InstanceManagementTable: React.FC<Props> = ({ refreshToken, reload }) => {

  const [api, contextHolder] = useLocalizedNotification();

  const actionReload = useCallback(() => {
    reload();
  }, [reload]);

  useInterval(reload, 5000);

  return (
    <InstanceTable refreshToken={refreshToken}>
      <Table.Column title={<Localized id={root.title} />}
        render={(_, instance: Instance) => (
          <Dropdown overlay={(
            <Menu>
              {action(root.start, () => service.startInstance(instance.id), actionReload, api)}
              {action(root.stop, () => service.stopInstance(instance.id), actionReload, api)}
              {action(root.restart, () => service.rebootInstance(instance.id, false), actionReload, api)}
              {action(root.hardRestart, () => service.rebootInstance(instance.id, true), actionReload, api)}
              {action(root.delete, () => service.deleteInstance(instance.id), actionReload, api)}
            </Menu>
          )}>
            <ClickableA>
              {contextHolder}
              <Localized id={root.title} /> <DownOutlined />
            </ClickableA>
          </Dropdown>
        )} />
    </InstanceTable>
  )
};

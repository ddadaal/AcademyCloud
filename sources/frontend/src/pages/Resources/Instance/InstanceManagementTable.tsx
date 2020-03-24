import React, { useCallback, useState, useRef, useEffect } from "react";
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
import { useAsync } from "react-async";
import { delay } from "src/utils/delay";

const root = lang.resources.instance.actions;
const opResult = lang.components.operationResult;

interface Props {
  refreshToken?: any;
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

const getInstances = () => service.getInstances().then(x => x.instances);

export const InstanceManagementTable: React.FC<Props> = ({ refreshToken }) => {

  const [api, contextHolder] = useLocalizedNotification();

  const { data, isPending, setData, reload } = useAsync({ promiseFn: getInstances, watch: refreshToken });

  const disposedRef = useRef<boolean | undefined>(false);

  const timedReload = useCallback(async () => {
    await delay(3000);
    try {
      const instances = await getInstances();
      setData(instances);
    }
    catch (e) {
      // ignored
    }
    if (!disposedRef.current) {
      timedReload();
    }
  }, [setData]);

  useEffect(() => {
    timedReload();
    return () => {
      disposedRef.current = true;
    };
  }, [timedReload]);

  return (
    <InstanceTable data={data} loading={isPending} >
      <Table.Column title={<Localized id={root.title} />}
        render={(_, instance: Instance) => (
          <Dropdown overlay={(
            <Menu>
              {action(root.start, () => service.startInstance(instance.id), reload, api)}
              {action(root.stop, () => service.stopInstance(instance.id), reload, api)}
              {action(root.restart, () => service.rebootInstance(instance.id, false), reload, api)}
              {action(root.hardRestart, () => service.rebootInstance(instance.id, true), reload, api)}
              {action(root.delete, () => service.deleteInstance(instance.id), reload, api)}
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

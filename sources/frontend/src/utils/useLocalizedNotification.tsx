import React from "react";
import { notification } from "antd";
import { ArgsProps } from "antd/lib/notification";
import { RecursiveLocalized } from "src/i18n";
import { RecursiveLocalizedId } from "src/i18n/RecursiveLocalized";

interface Props extends Omit<ArgsProps, "message"> {
  messageId: RecursiveLocalizedId;
  descriptionId?: RecursiveLocalizedId;
}

const createLocalizedIfNotNull = (id: RecursiveLocalizedId | undefined, targetKey: string) => {
  return id ? { [targetKey]: <RecursiveLocalized id={id} /> } : undefined;
}


export function useLocalizedNotification() {
  const [api, contextHolder] = notification.useNotification();

  const localizedConfig = (config: Props): ArgsProps => ({
    ...config,
    message: <RecursiveLocalized id={config.messageId} />,
    ...createLocalizedIfNotNull(config.descriptionId, "description"),
  });

  const localizedApi = Object.keys(api).reduce((prev, curr) => ({
    ...prev,
    [curr]: (config: Props) => api[curr](localizedConfig(config)),
  }), {}) as { [key in keyof typeof api]: (config: Props) => void };

  return [localizedApi, contextHolder] as const;
}

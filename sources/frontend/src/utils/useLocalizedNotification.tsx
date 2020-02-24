import React from "react";
import { notification } from "antd";
import { ArgsProps } from "antd/lib/notification";
import { Localized } from "src/i18n";

interface Props extends Omit<ArgsProps, "message"> {
  messageId: string;
  descriptionId?: string;
}

const createLocalizedIfNotNull = (id: string | undefined, targetKey: string) => {
  return id ? { [targetKey]: <Localized id={id} /> } : undefined
}


export function useLocalizedNotification() {
  const [api, contextHolder] = notification.useNotification();

  const localizedConfig = (config: Props): ArgsProps => ({
    ...config,
    message: <Localized id={config.messageId} />,
    ...createLocalizedIfNotNull(config.descriptionId, "description"),
  });

  const localizedApi = Object.keys(api).reduce((prev, curr) => ({
    ...prev,
    [curr]: (config: Props) => api[curr](localizedConfig(config)),
  }), {}) as { [key in keyof typeof api]: (config: Props) => void };

  return [localizedApi, contextHolder] as const;
}

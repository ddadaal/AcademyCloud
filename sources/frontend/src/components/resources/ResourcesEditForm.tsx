import React from "react";
import { Form, Input, Spin, Divider } from "antd";
import { Resources, ZeroResources } from "src/models/Resources";
import { lang, Localized, useLocalized } from "src/i18n";
import { useAsync } from "react-async";

interface Props {
  initial: Resources;
  onValuesChange: (resources: Resources) => void;
  getAvailableQuota: () => Promise<{ used: Resources; total: Resources }>;
}

const root = lang.components.resources;

const ResourceInput = ({ initial, used = ZeroResources, total = ZeroResources, resourceKey }: {
  initial: Resources;
  used?: Resources;
  total?: Resources;
  resourceKey: keyof Resources;
}) => (
  <Form.Item label={(
    <p>
      <strong><Localized id={root[resourceKey]} /></strong>
      <Divider type="vertical" />
      <Localized id={root.available} />
      <strong> {total[resourceKey] - used[resourceKey] + initial[resourceKey]}</strong>
      <Divider type="vertical" />
      <Localized id={root.total}  />
      <strong> {total[resourceKey]}</strong>
    </p>
  )} name={resourceKey}>
    <Input />
  </Form.Item>
);

export const ResourcesEditForm: React.FC<Props> = ({ initial, getAvailableQuota, onValuesChange }) => {

  const [form] = Form.useForm();

  const { data: quota, isPending } = useAsync({ promiseFn: getAvailableQuota });

  const loadingQuotaText = useLocalized(root.loadingQuota) as string;

  return (
    <Spin spinning={isPending} tip={loadingQuotaText}>
      <Form
        onValuesChange={(_, allValues) => onValuesChange(allValues as any)}
        form={form}
        initialValues={initial}
        layout="vertical"
      >
        <ResourceInput resourceKey="cpu" initial={initial} used={quota?.used} total={quota?.total} />
        <ResourceInput resourceKey="memory" initial={initial} used={quota?.used} total={quota?.total} />
        <ResourceInput resourceKey="storage" initial={initial} used={quota?.used} total={quota?.total} />
      </Form>
    </Spin>
  );
}

import React from "react";
import { Form, Input, Spin, Divider } from "antd";
import { Resources, ZeroResources } from "src/models/Resources";
import { lang, Localized, useLocalized } from "src/i18n";
import { useAsync } from "react-async";
import { required, integer } from "src/utils/validateMessages";
import { FormInstance } from "antd/lib/form";

interface Props {
  initial: Resources;
  getAvailableQuota: () => Promise<{ used: Resources; total: Resources }>;
  form: FormInstance;
}

const root = lang.components.resources;

const validator = async (rule, value) => {
  if (value === "") { return; }
  const num = Number(value);
  if (!Number.isInteger(num) || num > rule.max || num < rule.min) {
    throw new Error();
  }
}

const ResourceInput = ({ initial, used = ZeroResources, total = ZeroResources, resourceKey }: {
  initial: Resources;
  used?: Resources;
  total?: Resources;
  resourceKey: keyof Resources;
}) => {

  const available = total[resourceKey] - used[resourceKey] + initial[resourceKey];
  const usedAmount = used[resourceKey];

  return (
    <Form.Item label={(
      <p>
        <strong><Localized id={root[resourceKey]} /></strong>
        <Divider type="vertical" />
        <Localized id={root.available} />
        <strong> {available}</strong>
        <Divider type="vertical" />
        <Localized id={root.used} />
        <strong> {usedAmount}</strong>
        <Divider type="vertical" />
        <Localized id={root.total} />
        <strong> {total[resourceKey]}</strong>
      </p>
    )} name={resourceKey} rules={[
      { required: true, message: required },
      {
        max: available,
        min: usedAmount,
        message: <Localized id={root.number} />,
        validator,
      },
    ]}>
      <Input type="number" />
    </Form.Item>
  )
};

export const ResourcesEditForm: React.FC<Props> = ({ form, initial, getAvailableQuota }) => {

  const { data: quota, isPending } = useAsync({ promiseFn: getAvailableQuota });

  const loadingQuotaText = useLocalized(root.loadingQuota) as string;

  return (
    <Spin spinning={isPending} tip={loadingQuotaText}>
      <Form
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

import React, { useMemo } from "react";
import { Flavor, flavorString } from 'src/models/Instance';
import { Select, Form, Divider, Spin, Input } from 'antd';
import { getApiService } from 'src/apis';
import { InstanceService } from "src/apis/resources/InstanceService";
import { useAsync } from "react-async";
import { StrongLabel } from "src/components/StrongLabel";
import { Localized, lang } from "src/i18n";
import { minus, Resources, ZeroResources } from "src/models/Resources";
import { ResourcesService, GetResourcesUsedAndLimitsResponse } from "src/apis/resources/ResourcesService";
import { required } from "src/utils/validateMessages";

const service = getApiService(InstanceService);

const getFlavors = () => service.getFlavors().then(x => x.flavors);

const resourcesService = getApiService(ResourcesService);

const getLimits = () => resourcesService.getResourcesUsedAndLimits();

const flavorValidator = async (rule, { cpu, memory, storage }: Resources) => {
  if (cpu < 0 || memory < 0 || storage < 0) {
    throw new Error("form");
  }
}

const volumeValidator = async (rule, value) => {
  const num = Number(value);
  if (num > rule.max || num < rule.min) {
    throw new Error("form");
  }
}

const root = lang.resources.instance.add;

const calculateAvailable = (limits?: GetResourcesUsedAndLimitsResponse) => {
  return limits ? minus(limits.allocated, limits.used) : ZeroResources;
}

const fromFlavorToResources = (flavor?: Flavor) => {
  if (flavor) {
    return {
      cpu: flavor.cpu,
      memory: flavor.memory,
      storage: flavor.rootDisk,
    };
  } else {
    return ZeroResources;
  }
}

export const FlavorSelect: React.FC = (props) => {

  const { data: limits, isPending: limitsPending } = useAsync({ promiseFn: getLimits });
  const { data, isPending } = useAsync({ promiseFn: getFlavors });

  const available = useMemo(() => calculateAvailable(limits), [limits]);

  return (
    <>
      <Form.Item
        validateFirst={true}
        label={(
          <div>
            <StrongLabel id={root.flavor} />
            <Divider type="vertical" />
            <Localized id={root.available} />
            {` `}
            {limitsPending ? <Spin /> : `CPU ${available.cpu} | Memory ${available.memory} MB`}
          </div>
          // flavor is name
        )} name="flavor" rules={[
          { required: true, message: required },
          {
            validator: flavorValidator,
            transform: (name: string) => minus(
              available,
              fromFlavorToResources(data!.find(x => x.name === name))
            ),
            message: <Localized id={root.flavorLimit} />
          }
        ]}>
        <Select loading={isPending}>
          {(data ?? []).map(f => (
            <Select.Option key={f.name} value={f.name}>
              {flavorString(f)}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>
      <Form.Item
        validateFirst={true}
        label={
          <div>
            <StrongLabel id={root.volume} />
            <Divider type="vertical" />
            <Localized id={root.available} />
            {` `}
            {limitsPending ? <Spin /> : `${available.storage} GB`}
          </div>
        }
        name="volume"
        rules={[
          { required: true, message: required },
          { min: 0, max: available.storage, validator: volumeValidator, message: <Localized id={root.volumeLimit} /> }
        ]}
      >
        <Input type="number" />
      </Form.Item>
    </>
  )
};

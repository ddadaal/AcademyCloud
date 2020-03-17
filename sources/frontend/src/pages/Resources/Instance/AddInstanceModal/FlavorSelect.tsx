import React from "react";
import { Flavor, flavorString } from 'src/models/Instance';
import { Select, Form, Divider, Spin } from 'antd';
import { getApiService } from 'src/apis';
import { InstanceService } from "src/apis/resources/InstanceService";
import { useAsync } from "react-async";
import { StrongLabel } from "src/components/StrongLabel";
import { Localized, lang } from "src/i18n";
import { minus, Resources } from "src/models/Resources";
import { ResourcesService, GetResourcesUsedAndLimitsResponse } from "src/apis/resources/ResourcesService";
import { required } from "src/utils/validateMessages";

const service = getApiService(InstanceService);

const getFlavors = () => service.getFlavors().then(x => x.flavors);

const resourcesService = getApiService(ResourcesService);

const getLimits = () => resourcesService.getResourcesUsedAndLimits();

const flavorValidator = async (rule, { cpu, memory, storage }: Resources) => {
  if (cpu < 0 || memory < 0 || storage < 0) {
    throw new Error();
  }
}

const resourcesString = (resources: Resources) => {
  return `${resources.cpu} CPU | ${resources.memory * 1024} MB RAM | ${resources.storage} GB Storage`;
}

const root = lang.resources.instance.add;

const calculateAvailable = (limits?: GetResourcesUsedAndLimitsResponse) => {
  return minus(limits!.allocated, limits!.used);
}

const fromFlavorToResources = (flavor: Flavor) => {
  return {
    cpu: flavor.cpu,
    memory: flavor.memory / 1024,
    storage: flavor.rootDisk,
  };
}

export const FlavorSelect: React.FC = (props) => {

  const { data: limits, isPending: limitsPending } = useAsync({ promiseFn: getLimits });
  const { data, isPending } = useAsync({ promiseFn: getFlavors });

  return (
    <Form.Item label={(
      <div>
        <StrongLabel id={root.flavor} />
        <Divider type="vertical" />
        <Localized id={root.available} />
        {` `}
        {limitsPending ? <Spin /> : resourcesString(calculateAvailable(limits))}
      </div>
      // flavor is an id
    )} name="flavor" rules={[
      { required: true, message: required },
      {
        validator: flavorValidator,
        transform: (id: string) => minus(
          calculateAvailable(limits),
          fromFlavorToResources(data!.find(x => x.id === id)!)
        ),
        message: <Localized id={root.mustSmallerThanAvailable} />
      }
    ]}>
      <Select loading={isPending}>
        {(data ?? []).map(f => (
          <Select.Option key={f.id} value={f.id}>
            {flavorString(f)}
          </Select.Option>
        ))}
      </Select>
    </Form.Item>
  )
};

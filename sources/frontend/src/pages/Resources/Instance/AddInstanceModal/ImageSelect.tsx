import React from "react";
import { getApiService } from 'src/apis';
import { InstanceService } from 'src/apis/resources/InstanceService';
import { lang, Localized } from "src/i18n";
import { Select, Form } from "antd";
import { useAsync } from "react-async";
import { Instance, Image } from "src/models/Instance";
import { StrongLabel } from "src/components/StrongLabel";
import { required } from "src/utils/validateMessages";

const service = getApiService(InstanceService);

const getImages = () => service.getImages().then(x => x.images);

const root = lang.resources.instance.add;

export const ImageSelect: React.FC = (props) => {

  const { data, isPending } = useAsync({ promiseFn: getImages });

  return (
    <Form.Item
      name="image"
      label={<StrongLabel id={root.image} />}
      rules={[{ required: true, message: required }]}
    >
      <Select loading={isPending}>
        {(data ?? []).map(f => (
          <Select.Option key={f.id} value={f.id}>
            {f.name}
          </Select.Option>
        ))}
      </Select>
    </Form.Item>
  )
};

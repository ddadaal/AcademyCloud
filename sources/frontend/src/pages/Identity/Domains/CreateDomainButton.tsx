import React, { useState } from "react";
import { lang, Localized } from "src/i18n";
import { Button, Modal, Form, Input } from "antd";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { getApiService } from "src/apis";
import { DomainsService } from "src/apis/identity/DomainsService";
import { useAsync } from "react-async";
import { HttpError } from "src/apis/HttpService";

interface Props {
  reload: () => void;
}

const root = lang.identity.domains.add;
const opResult = lang.components.operationResult;

const service = getApiService(DomainsService);

const createDomain = ([name]) => {
  return service.createDomain(name);
}

export const CreateDomainButton: React.FC<Props> = (props) => {

  const [form] = Form.useForm();

  const [modalShown, setModalShown] = useState(false);

  const [api, contextHolder] = useLocalizedNotification();

  const { isPending, run } = useAsync({
    deferFn: createDomain,
    onResolve: () => {
      setModalShown(false);
      api.success({ messageId: [opResult.success, [root.opName]]});
      props.reload();
    },
    onReject: (e: any) => {
      const { status } = e.data as HttpError;
      api.error({
        messageId: [opResult.fail, [root.opName]],
        descriptionId: status === 403 ? root.conflict : opResult.fail,
      });
    }
  });

  return (
    <>
      <Button type="primary" onClick={() => setModalShown(true)}>
        <Localized id={root.button} />
      </Button>
      {contextHolder}
      <Modal
        visible={modalShown}
        title={<Localized id={root.title} />}
        onOk={() => run(form.getFieldsValue().name)}
        onCancel={() => setModalShown(false)}
        destroyOnClose={true}
        confirmLoading={isPending}
      >
        <Form form={form}>
          <Form.Item label={<Localized id={root.name}/>} rules={[{required: true}]} name="name">
            <Input />
          </Form.Item>
        </Form>

      </Modal>
    </>
  )
}

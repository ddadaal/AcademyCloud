import React, { useState } from "react";
import { lang, Localized } from "src/i18n";
import { Button, Modal, Form } from "antd";
import { PlusOutlined } from "@ant-design/icons";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { getApiService } from "src/apis";
import { useAsync } from "react-async";
import { required } from "src/utils/validateMessages";
import { PersonalAccountService } from "src/apis/identity/PersonalAccountService";
import { JoinableDomainSelectionTable } from "src/pages/Identity/Account/JoinedDomains/JoinableDomainSelectionTable";
import { AvailableScopesStore } from "src/stores/AvailableScopesStore";
import { useStore } from "simstate";

interface Props {
  reload: () => void;
}

const root = lang.identity.account.joinDomain;
const opResult = lang.components.operationResult;

const service = getApiService(PersonalAccountService);

const joinDomain = ([domainId]: [string]) => {
  return service.joinDomain(domainId);
}

const validator = async (_: any, value: object) => {
  if (!value) {
    throw new Error("[domain] is not set.");
  }
}

export const JoinDomainLink: React.FC<Props> = (props) => {

  const [form] = Form.useForm();

  const [modalShown, setModalShown] = useState(false);

  const [api, contextHolder] = useLocalizedNotification();

  const availableScopesStore = useStore(AvailableScopesStore);

  const { isPending: joining, run } = useAsync({
    deferFn: joinDomain,
    onResolve: () => {
      setModalShown(false);
      api.success({ messageId: [opResult.success, [root.opName]] });
      props.reload();
      availableScopesStore.updateScopes();
    },
    onReject: () => {
      api.error({
        messageId: [opResult.fail, [root.opName]],
        descriptionId: opResult.fail,
      });
    }
  });

  const onOk = () => {
    form.validateFields()
      .then((fields) => { run(fields.name, fields.payUser) })
      .catch((ex) => console.log(ex));
  }

  return (
    <>
      <Button type="primary" onClick={() => setModalShown(true)}>
        <PlusOutlined /> <Localized id={root.link} />
      </Button>
      {contextHolder}
      <Modal
        visible={modalShown}
        title={<Localized id={root.title} />}
        onOk={onOk}
        onCancel={() => setModalShown(false)}
        destroyOnClose={true}
        confirmLoading={joining}
      >
        <Form layout="vertical" form={form}>
          <Form.Item label={<Localized id={root.prompt} />}
            rules={[{
              required: true,
              validator: validator,
              message: required,
            }]} name="domain">
            <JoinableDomainSelectionTable />
          </Form.Item>
        </Form>

      </Modal>
    </>
  )
}

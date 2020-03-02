import React, { useState } from "react";
import { lang, Localized } from "src/i18n";
import { Button, Modal, Form, Input } from "antd";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { getApiService } from "src/apis";
import { DomainsService } from "src/apis/identity/DomainsService";
import { useAsync } from "react-async";
import { HttpError } from "src/apis/HttpService";
import { UsersSelectionMenu } from "src/components/users/UsersSelectionMenu";
import { UsersService } from "src/apis/identity/UsersService";
import { User } from "src/models/User";
import { required } from "src/utils/validateMessages";
import { arrayContainsElement } from "src/utils/Arrays";
import { ProjectsService } from "src/apis/identity/ProjectsService";

interface Props {
  reload: () => void;
}

const root = lang.identity.projects.create;
const opResult = lang.components.operationResult;

const usersService = getApiService(UsersService);
const getAccessibleUsers = () => usersService.getAccessibleUsers().then((x) => x.users);


const service = getApiService(ProjectsService);

const createProject = ([name, payUsers]: [string, User[]]) => {
  return service.createProject(name, payUsers[0].id);
}

const validator = async (_, value: User[] | undefined) => {
  if (!arrayContainsElement(value)) {
    throw new Error("[payUser] is not set.");
  }
}

export const CreateProjectButton: React.FC<Props> = (props) => {

  const [form] = Form.useForm();

  const [modalShown, setModalShown] = useState(false);

  const [api, contextHolder] = useLocalizedNotification();

  const { isPending, run } = useAsync({
    deferFn: createProject,
    onResolve: () => {
      setModalShown(false);
      api.success({ messageId: [opResult.success, [root.opName]] });
      props.reload();
    },
    onReject: (e: any) => {
      console.log(e);
      const { status } = e.data as HttpError;
      api.error({
        messageId: [opResult.fail, [root.opName]],
        descriptionId: status === 403 ? root.conflict : opResult.fail,
      });
    }
  });

  const onOk = () => {
    form.validateFields()
      .then((fields) =>  run(fields.name, fields.payUsers) )
      .catch((ex) => console.log(ex));
  }

  return (
    <>
      <Button type="primary" onClick={() => setModalShown(true)}>
        <Localized id={root.button} />
      </Button>
      {contextHolder}
      <Modal
        visible={modalShown}
        title={<Localized id={root.button} />}
        onOk={onOk}
        onCancel={() => setModalShown(false)}
        destroyOnClose={true}
        confirmLoading={isPending}
      >
        <Form layout="vertical" form={form}>
          <Form.Item label={<Localized id={root.name} />} rules={[{ required: true, message: required }]} name="name">
            <Input />
          </Form.Item>
          <Form.Item label={<Localized id={root.payUser} />}
            rules={[{
              required: true,
              validator: validator,
              message: required,
            }]} name="payUsers">
            <UsersSelectionMenu
              getUsers={getAccessibleUsers}
              selectionMode="single" />
          </Form.Item>
        </Form>
      </Modal>
    </>
  )
}

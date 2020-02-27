import React, { useState, useCallback } from "react";
import { User } from "src/models/User";
import { arrayContainsElement } from "src/utils/Arrays";
import { Button, Modal, Form } from "antd";
import { Localized, lang } from "src/i18n";
import { UsersSelectionMenu } from "src/components/users/UsersSelectionMenu";
import { RoleSelect } from "src/components/users/UserRoleEditTable/RoleSelect";
import { required } from "src/utils/validateMessages";
import { UserRole } from "src/models/Scope";
import { useAsync } from "react-async";

const root = lang.components.users;

interface Props {
  onAdd: (user: User, role: UserRole) => Promise<void>;
  getUsers: () => Promise<User[]>;
}

const validator = async (_, value: string[]) => {
  if (!arrayContainsElement(value)) {
    throw new Error("[user] is not set.");
  }
}

export const AddButton: React.FC<Props> = ({ onAdd, getUsers }) => {

  const [visible, setVisible] = useState(false);

  const [form] = Form.useForm();

  const deferFn = useCallback(([user, role]: [User, UserRole]) => onAdd(user, role), [onAdd]);

  const { run, isPending } = useAsync({
    deferFn,
    onResolve: () => {
      setVisible(false);
    },
  });

  const onOk = async () => {
    const { users, role } = await form.validateFields()
    run(users[0], role);
  };

  return (
    <>
      <Button type="primary" onClick={() => setVisible(true)}>
        <Localized id={root.addUser.button} />
      </Button>
      <Modal
        title={<Localized id={root.addUser.button} />}
        visible={visible}
        onOk={onOk}
        onCancel={() => setVisible(false)}
        destroyOnClose={true}
        confirmLoading={isPending}
      >
        <Form initialValues={{ user: [], role: "member" }} layout="vertical" form={form}>
          <Form.Item
            label={<Localized id={root.addUser.selectUser} />}
            rules={[{ required: true, validator, message: required }]}
            name="users"
          >
            <UsersSelectionMenu
              selectionMode="single"
              getUsers={getUsers}
            />
          </Form.Item>
          <Form.Item
            label={<Localized id={root.addUser.selectRole} />}
            rules={[{ required: true }]}
            name="role">
            <RoleSelect />
          </Form.Item>
        </Form>
      </Modal>
    </>
  )

}

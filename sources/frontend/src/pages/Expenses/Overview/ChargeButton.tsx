import React, { useState } from "react";
import { Modal, Form, Button, Input } from "antd";
import { Localized, lang } from "src/i18n";
import { integer } from "src/utils/validateMessages";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { getApiService } from "src/apis";
import { BalanceService } from "src/apis/expenses/BalanceService";

const root = lang.expenses.overview;

const opResult = lang.components.operationResult;

const service = getApiService(BalanceService);

const charge = (amount: number) => service.charge(amount).then((x) => x.balance);

interface Props {
  reload: (newBalance: number) => void;
}

export const ChargeButton: React.FC<Props> = ({ reload }) => {

  const [form] = Form.useForm();

  const [visible, setVisible] = useState(false);

  const [api, contextHolder] = useLocalizedNotification();

  const [charging, setCharing] = useState(false);

  const onOk = async () => {
    const { amount } = await form.validateFields();
    try {
      setCharing(true);
      const newBalance = await charge(amount);
      reload(newBalance);
      setVisible(false);
      api.success({ messageId: [opResult.success, [root.charge.opName]] });
    } finally {
      setCharing(false);
    }
  }

  return (
    <>
      <Button type="primary" onClick={() => setVisible(true)}>
        <Localized id={root.charge.button} />
      </Button>
      {contextHolder}
      <Modal
        title={<Localized id={root.charge.button} />}
        confirmLoading={charging}
        onOk={onOk}
        onCancel={() => setVisible(false)}
        visible={visible} >
        <Form layout="vertical" form={form} initialValues={{ amount: 0 }}>
          <Form.Item label={<Localized id={root.charge.amount} />} rules={[
            { type: "integer", required: true, message: integer, transform: (value) => Number(value) }
          ]} name="amount" >
            <Input />
          </Form.Item >
        </Form >
      </Modal >
    </>
  )
}

import React, { useState } from "react";
import { getApiService } from "src/apis";
import { OverviewService } from "src/apis/expenses/OverviewService";
import { useAsync } from "react-async";
import { Statistic, Spin, Button, Modal, Form, Input, Divider, Card, Empty } from "antd";
import { integer } from "src/utils/validateMessages";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { lang, Localized } from "src/i18n";
import { Link } from "@reach/router";
import styled from "styled-components";
import { MarginedCard } from "src/components/MarginedCard";

const EmptyDivider = styled.div`
  margin: 12px 0;
`;

const root = lang.expenses.overview;
const opResult = lang.components.operationResult;
const service = getApiService(OverviewService);

const getBalance = () => service.getBalance().then((x) => x.balance);
const charge = (amount: number) => service.charge(amount).then((x) => x.balance);

export const Balance: React.FC = (props) => {
  const [form] = Form.useForm();
  const [visible, setVisible] = useState(false);

  const [api, contextHolder] = useLocalizedNotification();

  const [charging, setCharing] = useState(false);

  const { data, isPending, setData } = useAsync({
    promiseFn: getBalance,
  });

  const onOk = async () => {
    const { amount } = await form.validateFields();
    try {
      setCharing(true);
      const newBalance = await charge(amount);
      setData(newBalance);
      setVisible(false);
      api.success({ messageId: [opResult.success, [root.charge.opName]] });
    } finally {
      setCharing(false);
    }
  }

  return (
    <MarginedCard title={<Localized id={root.balance} />} extra={(
      <Link to="../accountTransactions">
        <Localized id={root.toAccountTransaction} />
      </Link>
    )}>
      <Spin spinning={isPending}>
        <Statistic value={data ?? 0} precision={2} />
        <EmptyDivider />
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
              { type: "integer", required: true, message: integer, transform: (value) => Number(value) },
            ]} name="amount">
              <Input />
            </Form.Item>
          </Form>
        </Modal>
      </Spin>
    </MarginedCard>
  )
}

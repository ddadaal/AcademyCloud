import React, { useCallback } from "react";
import { CurrentAllocatedBilling, CurrentUsedBilling, BillSubjectType, BillType } from 'src/models/Billings';
import { Row, Col, Spin, Tooltip } from "antd";
import { MarginedCard } from "src/components/MarginedCard";
import { BillingService } from "src/apis/expenses/BillingService";
import { getApiService } from "src/apis";
import { useAsync } from "react-async";
import { ResourcesModalLink } from "src/components/resources/ResourcesModalLink";
import { LocalizedDate } from "src/i18n/LocalizedDate";
import { lang } from "src/i18n";
import { BillingStat } from "src/components/billings/BillingStat";

interface Props {
  refreshToken?: any;
  billType: BillType;
  billSubjectType: BillSubjectType;
  id: string;
}

const service = getApiService(BillingService);

const root = lang.components.billings.stats;

export const Stats: React.FC<Props> = ({ billType, billSubjectType, id, refreshToken }) => {

  const showPayer = billType === BillType.Allocated && billSubjectType !== BillSubjectType.User;

  const promiseFn = useCallback(async () => {
    const resp = billType === BillType.Allocated
      ? service.getCurrentAllocatedBilling(billSubjectType, id)
      : service.getCurrentUsedBilling(billSubjectType, id);
    return (await resp).billing;
  }, [billType, billSubjectType, id]);

  const { data, isPending } = useAsync({ promiseFn, watch: refreshToken });

  return (
    <Spin spinning={isPending}>
      <Row gutter={16}>
        <Col xs={24} sm={12} md={showPayer ? 6 : 8}>
          <BillingStat titleId={root.quota} data={data?.resources} >
            {data => <ResourcesModalLink resources={data} />}
          </BillingStat>
        </Col>
        <Col xs={24} sm={12} md={showPayer ? 6 : 8}>
          <BillingStat titleId={root.amount} data={data?.amount} >
            {data => <span>{data.toFixed(2)}</span>}
          </BillingStat>
        </Col>
        <Col xs={24} sm={showPayer ? 12 : 24} md={showPayer ? 6 : 8}>
          <BillingStat titleId={root.nextDue} data={data?.nextDue} >
            {data => <LocalizedDate dateTimeString={data} />}
          </BillingStat>
        </Col>
        {showPayer ? (
          // trust me
          <Col xs={24} sm={12} md={6}>
            <BillingStat titleId={root.payer} data={data} >
              {(data: CurrentAllocatedBilling) => (
                <Tooltip overlay={data.payerId}>
                  <span>{data.payerName}</span>
                </Tooltip>
              )}
            </BillingStat>
          </Col>
        ) : null}
      </Row>
    </Spin>
  )
}

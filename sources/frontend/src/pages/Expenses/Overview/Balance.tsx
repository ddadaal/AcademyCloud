import React from "react";
import { getApiService } from "src/apis";
import { OverviewService } from "src/apis/expenses/OverviewService";
import { useAsync } from "react-async";
import { Statistic, Spin } from "antd";
import { lang, Localized } from "src/i18n";
import { Link } from "@reach/router";
import styled from "styled-components";
import { MarginedCard } from "src/components/MarginedCard";
import { ChargeButton } from "src/pages/Expenses/Overview/ChargeButton";

const EmptyDivider = styled.div`
  margin: 12px 0;
`;

const root = lang.expenses.overview;
const service = getApiService(OverviewService);

const getBalance = () => service.getBalance().then((x) => x.balance);

export const Balance: React.FC = () => {

  const { data, isPending, setData } = useAsync({
    promiseFn: getBalance,
  });


  return (
    <MarginedCard title={<Localized id={root.balance} />} extra={(
      <Link to="../accountTransactions">
        <Localized id={root.toAccountTransaction} />
      </Link>
    )}>
      <Spin spinning={isPending}>
        <Statistic value={data ?? 0} precision={2} />
        <EmptyDivider />
      </Spin>
      <ChargeButton reload={setData} />
    </MarginedCard>
  )
}

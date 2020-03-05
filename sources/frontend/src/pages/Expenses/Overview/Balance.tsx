import React, { useCallback } from "react";
import { getApiService } from "src/apis";
import { OverviewService } from "src/apis/expenses/OverviewService";
import { useAsync } from "react-async";
import { Statistic, Spin, Divider } from "antd";
import { lang, Localized } from "src/i18n";
import { Link } from "@reach/router";
import styled from "styled-components";
import { MarginedCard } from "src/components/MarginedCard";
import { ChargeButton } from "src/pages/Expenses/Overview/ChargeButton";
import { BalanceTable } from "src/pages/Expenses/Overview/BalanceTable";
import { useRefreshToken } from "src/utils/refreshToken";

const EmptyDivider = styled.div`
  margin: 12px 0;
`;

const root = lang.expenses.overview;
const service = getApiService(OverviewService);

const getBalance = () => service.getBalance().then((x) => x.balance);

const BalanceRow = styled.div`
  display: flex;
  align-items: center;
`;

export const Balance: React.FC = () => {

  const { data, isPending, setData } = useAsync({
    promiseFn: getBalance,
  });

  const [token, refresh] = useRefreshToken();

  const reload = useCallback((amount: number) => {
    setData(amount);
    refresh();
  }, []);


  return (
    <MarginedCard title={<Localized id={root.balance} />} extra={(
      <Link to="../transactions/account">
        <Localized id={root.toAccountTransaction} />
      </Link>
    )}>
      <Spin spinning={isPending}>
        <BalanceRow>
          <Statistic value={data ?? 0} precision={2} />
          <Divider type="vertical" />
          <ChargeButton reload={reload} />
        </BalanceRow>
      </Spin>
      <EmptyDivider />
      <BalanceTable refreshToken={token} />
    </MarginedCard>
  )
}

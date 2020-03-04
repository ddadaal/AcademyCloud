import React from "react";
import { getApiService } from "src/apis";
import { TransactionsService } from "src/apis/expenses/TransactionsService";
import { useAsync, IfPending, IfFulfilled } from "react-async";
import { Spin, Card } from "antd";
import { OrgTransactionTable } from "src/components/transactions/OrgTransactionTable";
import { AntStatisticsTitle } from "src/components/AntStatsticsTitle";
import { Link } from "@reach/router";
import { MarginedCard } from "src/components/MarginedCard";

const transactionService = getApiService(TransactionsService);

const getTransactions = async () => {
  const resp = await transactionService.getSystemTransactions(5);
  return resp.transactions;
}

export const System: React.FC = () => {
  const { data, isPending } = useAsync({ promiseFn: getTransactions });

  return (
    <MarginedCard title={"Last 5 system transactions"} extra={<Link to="../systemTransactions">To systemTransactions</Link>}>
      <OrgTransactionTable data={data} loading={isPending} />
    </MarginedCard>
  )

}

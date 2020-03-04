import React from "react";
import { getApiService } from "src/apis";
import { AccountTransactionTable } from "src/components/transactions/AccountTransactionTable";
import { Spin } from "antd";
import { useAsync } from "react-async";
import { TransactionsService } from "src/apis/expenses/TransactionsService";

const service = getApiService(TransactionsService);

const getAccountTransactions = () => service.getAccountTransactions().then((x) => x.transactions);

interface Props {
  refreshToken: any;
}

export const Table: React.FC<Props> = ({ refreshToken }) => {
  const { data, isPending } = useAsync({ promiseFn: getAccountTransactions, watch: refreshToken });

  if (isPending) {
    return <Spin />;
  }

  return <AccountTransactionTable data={data!!} />;
}



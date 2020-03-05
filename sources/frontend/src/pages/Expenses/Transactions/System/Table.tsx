import React from "react";
import { getApiService } from "src/apis";
import { useAsync } from "react-async";
import { TransactionsService } from "src/apis/expenses/TransactionsService";
import { OrgTransactionTable } from 'src/components/transactions/OrgTransactionTable';

const service = getApiService(TransactionsService);

const getSystemTransactions = () => service.getSystemTransactions().then((x) => x.transactions);

interface Props {
  refreshToken: any;
}

export const Table: React.FC<Props> = ({ refreshToken }) => {
  const { data, isPending } = useAsync({ promiseFn: getSystemTransactions, watch: refreshToken });

  return <OrgTransactionTable data={data} loading={isPending} />;
}



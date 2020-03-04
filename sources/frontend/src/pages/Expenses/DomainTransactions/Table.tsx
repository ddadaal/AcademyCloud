import React, { useState, useCallback } from "react";
import { getApiService } from "src/apis";
import { Spin } from "antd";
import { useAsync } from "react-async";
import { TransactionsService } from "src/apis/expenses/TransactionsService";
import { OrgTransactionTable } from 'src/components/transactions/OrgTransactionTable';
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";

const service = getApiService(TransactionsService);


interface Props {
  refreshToken: any;
}

export const Table: React.FC<Props> = ({ refreshToken }) => {
  const { user } = useStore(UserStore);

  const getTransactions = useCallback(() => service.getDomainTransactions(user!!.scope.domainId).then((x) => x.transactions), [user]);

  const { data, isPending } = useAsync({ promiseFn: getTransactions, watch: refreshToken });

  return <OrgTransactionTable data={data} loading={isPending} />;
}



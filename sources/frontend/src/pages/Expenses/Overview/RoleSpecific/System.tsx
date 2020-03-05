import React from "react";
import { getApiService } from "src/apis";
import { TransactionsService } from "src/apis/expenses/TransactionsService";
import { useAsync, IfPending, IfFulfilled } from "react-async";
import { OrgTransactionTable } from "src/components/transactions/OrgTransactionTable";
import { Link } from "@reach/router";
import { MarginedCard } from "src/components/MarginedCard";
import { Section } from "src/pages/Expenses/Overview/Section";
import { lang, Localized } from "src/i18n";

const transactionService = getApiService(TransactionsService);

const root = lang.expenses.overview.system;


const getTransactions = async () => {
  const resp = await transactionService.getSystemTransactions(5);
  return resp.transactions;
}

export const System: React.FC = () => {
  const { data, isPending } = useAsync({ promiseFn: getTransactions });

  return (
    <Section
      title={<Localized id={root.title} />}
      extra={<Link to="../systemTransactions"><Localized id={root.link} /></Link>}
    >
      <OrgTransactionTable data={data} loading={isPending} />
    </Section>
  )

}

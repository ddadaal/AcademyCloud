import React from "react";
import { RouteComponentProps } from "@reach/router";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { TransactionsTable } from "src/pages/Expenses/AccountTransactions/TransactionsTable";
import { lang, Localized } from "src/i18n";
import { useRefreshToken } from "src/utils/refreshToken";
import { ClickableA } from "src/components/ClickableA";

const root = lang.expenses.accountTransactions;

export const Transactions: React.FC<RouteComponentProps> = (props) => {

  const [token, refresh] = useRefreshToken();

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText><Localized id={root.title} /></TitleText>
        <ClickableA onClick={refresh}><Localized id={root.refresh} /></ClickableA>
      </TitleBar>
      <TransactionsTable refreshToken={token} />
    </div>

  );
}

export default Transactions;

import React from "react";
import { RouteComponentProps } from "@reach/router";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { lang, Localized } from "src/i18n";
import { useRefreshToken } from "src/utils/refreshToken";
import { ClickableA } from "src/components/ClickableA";
import { Table } from "./Table";

const root = lang.expenses.domainTransactions;

export const DomainTransactions: React.FC<RouteComponentProps> = () => {

  const [token, refresh] = useRefreshToken();

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText><Localized id={root.title} /></TitleText>
        <ClickableA onClick={refresh}><Localized id={root.refresh} /></ClickableA>
      </TitleBar>
      <Table refreshToken={token} />
    </div>

  );
}

export default DomainTransactions;

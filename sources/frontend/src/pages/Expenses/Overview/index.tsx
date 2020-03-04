import React from "react";
import { RouteComponentProps } from "@reach/router";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { Balance } from "src/pages/Expenses/Overview/Balance";
import { RoleSpecific } from "./RoleSpecific";
import { lang, Localized } from "src/i18n";

const root = lang.expenses.overview;

export const Overview: React.FC<RouteComponentProps> = (props) => {
  return (
    <div>
      <TitleBar>
        <TitleText>
          <Localized id={root.root} />
        </TitleText>
      </TitleBar>
      <Balance />
      <RoleSpecific />
    </div >
  );
}

export default Overview;

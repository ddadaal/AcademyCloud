import React from "react";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { lang, Localized } from "src/i18n";
import { TitleText } from "src/components/pagecomponents/TitleText";

const root = lang.identity.account.joinDomain;


export const JoinDomainPage: React.FC = (props) => {
  return (
    <div>
      <TitleBar>
        <TitleText>
          <Localized id={root.title} />
        </TitleText>
      </TitleBar>
    </div>
  );
}

export default JoinDomainPage;

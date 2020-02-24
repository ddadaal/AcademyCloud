import React from "react";
import { RouteComponentProps } from "@reach/router";
import { lang, Localized } from 'src/i18n';
import { TitleText } from 'src/components/pagecomponents/TitleText';
import { TitleBar } from 'src/components/pagecomponents/TitleBar';
import { DomainsTable } from "src/pages/Identity/Domains/DomainsTable";

const root = lang.identity.domains;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default function DomainsPage(_: RouteComponentProps) {
  return (
    <div>
      <TitleBar>
        <TitleText><Localized id={root.title} /></TitleText>
      </TitleBar>
      <DomainsTable />
    </div>
  );
}


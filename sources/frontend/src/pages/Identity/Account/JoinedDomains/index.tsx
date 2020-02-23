import React from "react";
import { RouteComponentProps } from "@reach/router";
import { PageTitle } from "src/components/PageTitle";
import { lang, LocalizedString } from "src/i18n";
import { DomainsTable } from "src/pages/Identity/Account/JoinedDomains/DomainsTable";

const root = lang.identity.account.joinedDomains;


// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default function JoinedDomainsPage(_: RouteComponentProps) {

  return (
    <div>
      <PageTitle><LocalizedString id={root.title} /></PageTitle>
      <DomainsTable />
    </div>
  );
}


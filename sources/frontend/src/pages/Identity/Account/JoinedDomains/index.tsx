import React from "react";
import { RouteComponentProps } from "@reach/router";
import { PageTitle } from "src/components/PageTitle";
import { lang, LocalizedString } from "src/i18n";
import { DomainsTable } from "src/pages/Identity/Account/JoinedDomains/DomainsTable";
import styled from 'styled-components';
import { Button } from "antd";

const root = lang.identity.account.joinedDomains;


const TitleBar = styled.div`
  display: flex;
  justify-content: space-between;
  margin: 16px 0;
`;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default function JoinedDomainsPage(_: RouteComponentProps) {

  return (
    <div>
      <TitleBar>
        <PageTitle><LocalizedString id={root.title} /></PageTitle>
        <Button type="primary">
          <LocalizedString id={root.join} />
        </Button>
      </TitleBar>
      <DomainsTable />
    </div>
  );
}


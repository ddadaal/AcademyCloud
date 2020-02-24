import React from "react";
import { RouteComponentProps } from "@reach/router";
import { lang, LocalizedString } from "src/i18n";
import { DomainsTable } from "src/pages/Identity/Account/JoinedDomains/DomainsTable";
import styled from 'styled-components';
import { Button } from "antd";
import { TitleText } from "src/components/pagecomponents/PageTitle";
import { TitleBar } from "src/components/pagecomponents/TitleBar";

const root = lang.identity.account.joinedDomains;


const SpaceBetweenTitleBar = styled(TitleBar)`
  display: flex;
  justify-content: space-between;
  align-items: center;
`;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default function JoinedDomainsPage(_: RouteComponentProps) {

  return (
    <div>
      <SpaceBetweenTitleBar>
        <TitleText><LocalizedString id={root.title} /></TitleText>
        <Button type="primary">
          <LocalizedString id={root.join} />
        </Button>
      </SpaceBetweenTitleBar>
      <DomainsTable />
    </div>
  );
}


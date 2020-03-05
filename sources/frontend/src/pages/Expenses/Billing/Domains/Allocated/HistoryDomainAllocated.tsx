import React from "react";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { DomainsAllocatedTable } from "src/pages/Expenses/Billing/Domains/Allocated/DomainsAllocatedTable";
import { useRefreshToken } from "src/utils/refreshToken";
import { Localized, lang } from "src/i18n";
import { ClickableA } from "src/components/ClickableA";
import { RouteComponentProps, navigate, Link } from "@reach/router";
import { StepBackwardOutlined } from "@ant-design/icons";
import { HistoryDomainAllocatedTable } from "src/pages/Expenses/Billing/Domains/Allocated/HistoryDomainAllocatedTable";
import { Button, Divider } from "antd";
import styled from "styled-components";

type Props = RouteComponentProps<{ domainId: string }>;

const FlexBox = styled.div`
  display: flex;
`;

const root = lang.expenses.billings;


export const HistoryDomainAllocated: React.FC<Props> = ({ domainId, location }) => {

  const [token, refresh] = useRefreshToken();

  // just parse the first arg and use it as the name
  const name = location?.search?.replace?.(/^.*?=/, '');

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <FlexBox>
          <TitleText>
            <Button>
              <Link to=".."><StepBackwardOutlined /></Link>
            </Button>
            <Divider type="vertical" />
            <Localized id={root.domainHistoryAllocated} replacements={[name || domainId]}/>
          </TitleText>
        </FlexBox>
        <ClickableA onClick={refresh}>Refresh</ClickableA>
      </TitleBar>
      <HistoryDomainAllocatedTable domainId={domainId!!} refreshToken={token} />
    </div>
  );
}

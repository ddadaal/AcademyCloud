import React from "react";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { DomainsAllocatedTable } from "src/pages/Expenses/Billing/Domains/Allocated/DomainsAllocatedTable";
import { useRefreshToken } from "src/utils/refreshToken";
import { Localized } from "src/i18n";
import { ClickableA } from "src/components/ClickableA";
import { RouteComponentProps, navigate, Link } from "@reach/router";
import { StepBackwardOutlined } from "@ant-design/icons";
import { HistoryDomainAllocatedTable } from "src/pages/Expenses/Billing/Domains/Allocated/HistoryDomainAllocatedTable";
import { Button, Divider } from "antd";

type Props = RouteComponentProps<{ domainId: string }>;

export const HistoryDomainAllocated: React.FC<Props> = ({ domainId }) => {

  const [token, refresh] = useRefreshToken();

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <div style={{ display: "flex" }}>
          <TitleText>
            <Button>
              <Link to=".."><StepBackwardOutlined /></Link>
            </Button>
            <Divider type="vertical" />
            Domain {domainId} HistoryAllocated
          </TitleText>
        </div>
        <ClickableA onClick={refresh}>Refresh</ClickableA>
      </TitleBar>
      <HistoryDomainAllocatedTable domainId={domainId!!} refreshToken={token} />
    </div>
  );
}

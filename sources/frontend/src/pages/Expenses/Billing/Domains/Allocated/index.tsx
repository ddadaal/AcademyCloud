import React from "react";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { DomainsAllocatedTable } from "src/pages/Expenses/Billing/Domains/Allocated/DomainsAllocatedTable";
import { useRefreshToken } from "src/utils/refreshToken";
import { Localized } from "src/i18n";
import { ClickableA } from "src/components/ClickableA";

export const DomainAllocated: React.FC = (props) => {

  const [token, refresh] = useRefreshToken();

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText>Domain Allocated</TitleText>
        <ClickableA onClick={refresh}>Refresh</ClickableA>
      </TitleBar>
      <DomainsAllocatedTable refreshToken={token} />
    </div>
  );
}

export default DomainAllocated;

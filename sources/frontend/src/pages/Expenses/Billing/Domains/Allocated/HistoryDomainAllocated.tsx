import React, { useCallback } from "react";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { Localized, lang } from "src/i18n";
import { ClickableA } from "src/components/ClickableA";
import { RouteComponentProps, Link } from "@reach/router";
import { StepBackwardOutlined } from "@ant-design/icons";
import { Button, Divider } from "antd";
import { HistoryBillingsTable } from "src/components/billings/HistoryBillingsTable";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { useAsync } from "react-async";
import { FlexBox } from "src/components/Flexbox";

type Props = RouteComponentProps<{ domainId: string }>;

const root = lang.expenses.billings;

const service = getApiService(BillingService);

export const HistoryDomainAllocated: React.FC<Props> = ({ domainId, location }) => {

  // just parse the first arg and use it as the name
  const name = location?.search?.replace?.(/^.*?=/, '');
  const promiseFn = useCallback(async () => {
    const resp = await service.getDomainHistoryAllocatedBillings(domainId!!);
    return resp.billings;
  }, [domainId]);

  const { data, isPending, reload } = useAsync({ promiseFn });

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <FlexBox>
          <TitleText>
            <Button>
              <Link to=".."><StepBackwardOutlined /></Link>
            </Button>
            <Divider type="vertical" />
            <Localized id={root.domainHistoryAllocated} replacements={[name || domainId]} />
          </TitleText>
        </FlexBox>
        <ClickableA onClick={reload}><Localized id={root.refresh} /></ClickableA>
      </TitleBar>
      <HistoryBillingsTable data={data} loading={isPending} />
    </div>
  );
}

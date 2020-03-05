import React, { useMemo } from "react";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { Localized, lang } from "src/i18n";
import { ClickableA } from "src/components/ClickableA";
import { RouteComponentProps } from "@reach/router";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { useAsync } from "react-async";
import { CurrentBillingsTable } from "src/components/billings/CurrentBillingsTable";

const root = lang.expenses.billings;

const service = getApiService(BillingService);

const getDomains = () => service.getDomainsCurrentAllocatedBilling().then(x => x.billings);

export const DomainsAllocated: React.FC<RouteComponentProps> = (props) => {

  const { data, isPending, reload } = useAsync({ promiseFn: getDomains });

  const processedData = useMemo(() => {
    return data?.map(x => ({ ...x, historyLink: `./${x.subjectId}?name=${x.subjectName}` }))
  }, [data])

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText><Localized id={root.domainsAllocated} /></TitleText>
        <ClickableA onClick={reload}><Localized id={root.refresh} /></ClickableA>
      </TitleBar>
      <CurrentBillingsTable subjectType="domain" data={processedData} loading={isPending} />
    </div>
  );
}

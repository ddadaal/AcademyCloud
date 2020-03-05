import React from "react";
import { Router, RouteComponentProps } from "@reach/router";
import { BillSubjectType, BillType } from 'src/models/Billings';
import { HistoryBillingsPage } from "src/components/billings/HistoryBillingsPage";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { CurrentBillingsPage } from "src/components/billings/CurrentBillingsPage";

const service = getApiService(BillingService);

const getDomains = () => service.getCurrentUsedBillings(BillSubjectType.domain).then(x => x.billings);

const DomainsUsed: React.FC<RouteComponentProps> = () => {
  return (
    <CurrentBillingsPage
      subjectType={BillSubjectType.domain}
      billType={BillType.Used}
      promiseFn={getDomains} />
  );
}
const getDomainHistoryData = async (id: string) => {
  const resp = await service.getHistoryUsedBillings(BillSubjectType.domain, id);
  return resp.billings;
};

const HistoryDomainUsed: React.FC<RouteComponentProps<{ domainId: string }>> = ({ domainId, location }) => {
  return (
    <HistoryBillingsPage id={domainId!!} billType={BillType.Used} subjectType={BillSubjectType.domain} getData={getDomainHistoryData} location={location!!} />
  );
}

export default function DomainUsed() {
  return (
    <Router>
      <DomainsUsed path="/" />
      <HistoryDomainUsed path=":domainId" />
    </Router>
  )
}

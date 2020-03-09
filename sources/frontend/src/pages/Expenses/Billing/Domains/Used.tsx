import React from "react";
import { Router, RouteComponentProps } from "@reach/router";
import { BillSubjectType, BillType } from 'src/models/Billings';
import { HistoryBillingsPage } from "src/components/billings/HistoryBillingsPage";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { CurrentBillingsPage } from "src/components/billings/CurrentBillingsPage";

const service = getApiService(BillingService);

const getDomains = () => service.getCurrentUsedBillings(BillSubjectType.Domain).then(x => x.billings);

const DomainsUsed: React.FC<RouteComponentProps> = () => {
  return (
    <CurrentBillingsPage
      subjectType={BillSubjectType.Domain}
      billType={BillType.Used}
      promiseFn={getDomains} />
  );
}
const getDomainHistoryData = async (id: string) => {
  const resp = await service.getHistoryUsedBillings(BillSubjectType.Domain, id);
  return resp.billings;
};

const HistoryDomainUsed: React.FC<RouteComponentProps<{ domainId: string }>> = ({ domainId, location }) => {
  return (
    <HistoryBillingsPage id={domainId!!} billType={BillType.Used} subjectType={BillSubjectType.Domain} getData={getDomainHistoryData} location={location!!} />
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

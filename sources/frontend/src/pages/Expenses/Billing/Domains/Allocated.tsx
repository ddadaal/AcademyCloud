import React from "react";
import { Router, RouteComponentProps } from "@reach/router";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { HistoryBillingsPage } from "src/components/billings/HistoryBillingsPage";
import { BillType, BillSubjectType } from "src/models/Billings";
import { CurrentBillingsPage } from "src/components/billings/CurrentBillingsPage";

const service = getApiService(BillingService);

const getDomains = () => service.getCurrentAllocatedBillings(BillSubjectType.Domain).then(x => x.billings);

const DomainsAllocated: React.FC<RouteComponentProps> = () => {
  return (
    <CurrentBillingsPage billType={BillType.Allocated} subjectType={BillSubjectType.Domain} promiseFn={getDomains} />
  );
}

const getData = (id: string) => service.getHistoryAllocatedBillings(BillSubjectType.Domain, id).then(x => x.billings);

const HistoryDomainAllocated: React.FC<RouteComponentProps<{ domainId: string }>> = ({ domainId, location }) => {

  return (
    <HistoryBillingsPage
      id={domainId!!}
      billType={BillType.Allocated}
      subjectType={BillSubjectType.Domain}
      getData={getData}
      location={location!!}
    />
  )
}
export default function () {
  return (
    <Router>
      <DomainsAllocated path="/" />
      <HistoryDomainAllocated path=":domainId" />
    </Router>
  )
}

import React from "react";
import { Router, RouteComponentProps } from "@reach/router";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { HistoryBillingsPage } from "src/components/billings/HistoryBillingsPage";
import { BillType, BillSubjectType } from "src/models/Billings";
import { CurrentBillingsPage } from "src/components/billings/CurrentBillingsPage";

const service = getApiService(BillingService);

const getProjects = () => service.getCurrentAllocatedBillings(BillSubjectType.project).then(x => x.billings);

const ProjectsAllocated: React.FC<RouteComponentProps> = () => {
  return (
    <CurrentBillingsPage billType={BillType.Allocated} subjectType={BillSubjectType.project} promiseFn={getProjects} />
  );
}

const getData = (id: string) => service.getHistoryAllocatedBillings(BillSubjectType.project, id).then(x => x.billings);

const HistoryProjectAllocated: React.FC<RouteComponentProps<{ projectId: string }>> = ({ projectId, location }) => {

  return (
    <HistoryBillingsPage
      id={projectId!!}
      billType={BillType.Allocated}
      subjectType={BillSubjectType.project}
      getData={getData}
      location={location!!}
    />
  )
}
export default function () {
  return (
    <Router>
      <ProjectsAllocated path="/" />
      <HistoryProjectAllocated path=":projectId" />
    </Router>
  )
}

import React from "react";
import { Router, RouteComponentProps } from "@reach/router";
import { BillSubjectType, BillType } from 'src/models/Billings';
import { HistoryBillingsPage } from "src/components/billings/HistoryBillingsPage";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { CurrentBillingsPage } from "src/components/billings/CurrentBillingsPage";

const service = getApiService(BillingService);

const getProjects = () => service.getCurrentUsedBillings(BillSubjectType.project).then(x => x.billings);

const ProjectsUsed: React.FC<RouteComponentProps> = () => {
  return (
    <CurrentBillingsPage
      subjectType={BillSubjectType.project}
      billType={BillType.Used}
      promiseFn={getProjects} />
  );
}
const getProjectHistoryData = async (id: string) => {
  const resp = await service.getHistoryUsedBillings(BillSubjectType.project, id);
  return resp.billings;
};

const HistoryProjectUsed: React.FC<RouteComponentProps<{ projectId: string }>> = ({ projectId, location }) => {
  return (
    <HistoryBillingsPage id={projectId!!} billType={BillType.Used} subjectType={BillSubjectType.project} getData={getProjectHistoryData} location={location!!} />
  );
}

export default function ProjectUsed() {
  return (
    <Router>
      <ProjectsUsed path="/" />
      <HistoryProjectUsed path=":projectId" />
    </Router>
  )
}

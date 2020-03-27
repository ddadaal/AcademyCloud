import React from "react";
import { Router, RouteComponentProps } from "@reach/router";
import { BillSubjectType, BillType } from 'src/models/Billings';
import { HistoryBillingsPage } from "src/components/billings/HistoryBillingsPage";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { CurrentBillingsPage } from "src/components/billings/CurrentBillingsPage";

const service = getApiService(BillingService);

const getusers = () => service.getCurrentUsedBillings(BillSubjectType.UserProjectAssignment).then(x => x.billings);

const UsersUsed: React.FC<RouteComponentProps> = () => {
  return (
    <CurrentBillingsPage
      subjectType={BillSubjectType.UserProjectAssignment}
      billType={BillType.Used}
      promiseFn={getusers} />
  );
}
const getUserHistoryData = async (id: string) => {
  const resp = await service.getHistoryUsedBillings(BillSubjectType.UserProjectAssignment, id);
  return resp.billings;
};

const HistoryuserUsed: React.FC<RouteComponentProps<{ userId: string }>> = ({ userId, location }) => {
  return (
    <HistoryBillingsPage
      id={userId!!}
      billType={BillType.Used}
      subjectType={BillSubjectType.UserProjectAssignment}
      getData={getUserHistoryData}
      location={location!!}
    />
  );
}

export default function userUsed() {
  return (
    <Router>
      <UsersUsed path="/" />
      <HistoryuserUsed path=":userId" />
    </Router>
  )
}

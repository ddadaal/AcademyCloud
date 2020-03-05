import React from "react";
import { Router, RouteComponentProps } from "@reach/router";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { HistoryBillingsPage } from "src/components/billings/HistoryBillingsPage";
import { BillType, BillSubjectType } from "src/models/Billings";
import { CurrentBillingsPage } from "src/components/billings/CurrentBillingsPage";

const service = getApiService(BillingService);

const getUsers = () => service.getCurrentAllocatedBillings(BillSubjectType.user).then(x => x.billings);

const UsersAllocated: React.FC<RouteComponentProps> = () => {
  return (
    <CurrentBillingsPage
      billType={BillType.Allocated}
      subjectType={BillSubjectType.user}
      promiseFn={getUsers}
    />
  );
}

const getData = (id: string) => service.getHistoryAllocatedBillings(BillSubjectType.user, id).then(x => x.billings);

const HistoryUserAllocated: React.FC<RouteComponentProps<{ userId: string }>> = ({ userId, location }) => {
  return (
    <HistoryBillingsPage
      id={userId!!}
      billType={BillType.Allocated}
      subjectType={BillSubjectType.user}
      getData={getData}
      location={location!!}
    />
  )
}
export default function () {
  return (
    <Router>
      <UsersAllocated path="/" />
      <HistoryUserAllocated path=":userId" />
    </Router>
  )
}

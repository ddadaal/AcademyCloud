import React from "react";
import { Result, Button } from 'antd';
import { lang, Localized } from 'src/i18n';
import { RouteComponentProps, Link } from "@reach/router";

const root = lang.resources.notAvailable;

export enum NotAvailableReason {
  NotProjectScope = "NotProjectScope",
  UserNotActive = "UserNotActive",
  ScopeNotActive = "ScopeNotActive",
}

interface Props extends RouteComponentProps {
  reason: NotAvailableReason;
}

export const ResourcesNotAvailable: React.FC<Props> = ({ reason }) => {

  return (
    <Result
      status={403}
      title={<Localized id={root[reason].title} />}
      subTitle={<Localized id={root[reason].subTitle} />}
      extra={<Button type="primary"><Link to="/expenses"><Localized id={root.toExpenses} /></Link></Button>}
    />
  )
};

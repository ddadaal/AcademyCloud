import React from "react";
import { Result, Button } from 'antd';
import { lang, Localized } from 'src/i18n';
import { RouteComponentProps, Link } from "@reach/router";

const root = lang.resources.notAvailable;

export const ResourcesNotAvailable: React.FC<RouteComponentProps> = () => {
  return (
    <Result
      status={403}
      title={<Localized id={root.title} />}
      subTitle={<Localized id={root.description} />}
      extra={<Button type="primary"><Link to="/expenses"><Localized id={root.toExpenses} /></Link></Button>}
    />
  )
};

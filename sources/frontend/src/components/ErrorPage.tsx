import React from "react";
import { Result, Button } from "antd";
import { RouteComponentProps, Link } from "@reach/router";
import { lang, Localized } from "src/i18n";

const root = lang.nav.errorPage;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export const ErrorPage = (_: RouteComponentProps) => {
  return (
    <Result
      status="404"
      title={<Localized id={root.title} />}
      subTitle={<Localized id={root.description} />}
      extra={(
        <Button type="primary" >
          <Link to="/resources">
            <Localized id={root.backToHome} />
          </Link>
        </Button>
      )}
    />
  )
}

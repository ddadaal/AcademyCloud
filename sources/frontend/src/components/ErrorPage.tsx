import React from "react";
import { Result, Button } from "antd";
import { RouteComponentProps, navigate, Link } from "@reach/router";
import { lang, LocalizedString } from "src/i18n";

const root = lang.nav.errorPage;


// eslint-disable-next-line @typescript-eslint/no-unused-vars
export const ErrorPage = (_: RouteComponentProps) => {
  return (
    <Result
      status="404"
      title={<LocalizedString id={root.title} />}
      subTitle={<LocalizedString id={root.description} />}
      extra={(
        <Button type="primary" >
          <Link to="/resources">
            <LocalizedString id={root.backToHome} />
          </Link>
        </Button>
      )}
    />
  )
}

import React, { Suspense } from "react";
import RootLayout from "src/layouts/RootLayout";
import FunctionLayout from "src/layouts/FunctionLayout";
import { Router, RouteComponentProps } from "@reach/router";
import PageLoading from "src/components/PageLoading";

const ResourcesPage = React.lazy(() => import("./Resources"));

export default function NormalPages(props: RouteComponentProps) {
  return (
    <RootLayout>
      <FunctionLayout>
        <Suspense fallback={<PageLoading />}>
          <Router>
            <ResourcesPage path="resources/*"/>
          </Router>
        </Suspense>
      </FunctionLayout>
    </RootLayout>
  );
}

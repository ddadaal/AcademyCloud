import React, { Suspense } from "react";
import RootLayout from "src/layouts/RootLayout";
import FunctionLayout from "src/layouts/FunctionLayout";
import { Router, RouteComponentProps, Redirect } from "@reach/router";
import PageLoading from "src/components/PageLoading";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { ErrorPage } from "src/components/ErrorPage";

const ResourcesPage = React.lazy(() => import("./Resources"));
const IdentityPage = React.lazy(() => import("./Identity"));
const ExpensesPage = React.lazy(() => import("./Expenses"));


const NormalPages: React.FC<RouteComponentProps> = () => {
  const userStore = useStore(UserStore);

  if (!userStore.loggedIn) {
    return (
      <Redirect noThrow={true} to="/" />
    );
  }

  return (
    <RootLayout>
      <FunctionLayout>
        <Suspense fallback={<PageLoading />}>
          <Router>
            <ResourcesPage path="resources/*" />
            <ExpensesPage path="expenses/*" />
            <IdentityPage path="identity/*" />
            <ErrorPage path="*" />
          </Router>
        </Suspense>
      </FunctionLayout>
    </RootLayout>
  );
}

export default NormalPages;

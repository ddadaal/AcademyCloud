import React, { Suspense } from "react";
import RootLayout from "src/layouts/RootLayout";
import FunctionLayout from "src/layouts/FunctionLayout";
import { Router, RouteComponentProps, Redirect } from "@reach/router";
import PageLoading from "src/components/PageLoading";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { ErrorPage } from "src/components/ErrorPage";
import { isResourcesDisabled } from 'src/models/Scope';
import { ResourcesNotAvailable, NotAvailableReason } from "src/pages/Resources/ResourcesNotAvailable";

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

  const { scope, scopeActive, userActive } = userStore.user!;

  // judge whether resources available
  const reason = isResourcesDisabled(scope) ? NotAvailableReason.NotProjectScope :
    (!userActive) ? NotAvailableReason.UserNotActive :
      (!scopeActive) ? NotAvailableReason.ScopeNotActive :
        null;

  return (
    <RootLayout>
      <FunctionLayout>
        <Suspense fallback={<PageLoading />}>
          <Router>
            {reason
              ? <ResourcesNotAvailable reason={reason} path="resources/*" />
              : <ResourcesPage path="resources/*" />
            }
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

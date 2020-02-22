import React, { Suspense } from "react";
import RootLayout from "src/layouts/RootLayout";
import FunctionLayout from "src/layouts/FunctionLayout";
import { Router, RouteComponentProps, Redirect } from "@reach/router";
import PageLoading from "src/components/PageLoading";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";

const ResourcesPage = React.lazy(() => import("./Resources"));
const IdentityPage = React.lazy(() => import("./Identity"));

const NormalPages: React.FC<RouteComponentProps> = () => {
  const userStore = useStore(UserStore);
  console.log(userStore);

  if (!userStore.loggedIn) {
    return (
      <Redirect to="/" />
    );
  }

  return (
    <RootLayout>
      <FunctionLayout>
        <Suspense fallback={<PageLoading />}>
          <Router>
            <ResourcesPage path="resources/*" />
            <IdentityPage path="identity/*" />
          </Router>
        </Suspense>
      </FunctionLayout>
    </RootLayout>
  );
}

export default NormalPages;

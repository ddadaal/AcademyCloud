import React, { Suspense } from 'react';
import { Router, RouteComponentProps } from "@reach/router";
import { StoreProvider, createStore } from 'simstate';
import { UserStore } from './stores/UserStore';
import { I18nStore } from "./i18n";
import PageLoading from "./components/PageLoading";
import { NavStore } from "./layouts/nav/NavStore";

const AsyncHomePages = React.lazy(() => import("./pages/Home"));
const AsyncNormalPages = React.lazy(() => import("./pages/NormalPages"));

// react router does not support regex matching, so
const TopLevelRouteSelector: React.FC<RouteComponentProps> = (props) => {
  // Location is present you bad type definitions
  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  const location = props.location!!;

  switch (location.pathname) {
    case "/":
    case "/register":
      return <AsyncHomePages />;
    default:
      return <AsyncNormalPages />;
  }
}

function App() {
  const userStore = createStore(UserStore);
  const i18nStore = createStore(I18nStore);
  const navStore = createStore(NavStore);

  return (
    <StoreProvider stores={[userStore, i18nStore, navStore]}>
      <Suspense fallback={<PageLoading />}>
        <Router primary={false}>
          <TopLevelRouteSelector path="*" />
        </Router>
      </Suspense>

    </StoreProvider>
  );
}

export default App;

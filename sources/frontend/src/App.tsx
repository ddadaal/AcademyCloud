import React, { Suspense, useMemo } from 'react';
import { Router, RouteComponentProps } from "@reach/router";
import { StoreProvider, createStore, useStore } from 'simstate';
import { UserStore } from './stores/UserStore';
import { I18nStore } from "./i18n";
import PageLoading from "./components/PageLoading";
import { NavStore } from "./layouts/nav/NavStore";
import { ConfigProvider } from "antd";

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

function AntdConfigProvider({ children }) {
  const i18nStore = useStore(I18nStore);
  const configs = useMemo(() => (
    i18nStore.currentLanguage.metadata.antdConfigProvider
  ), [i18nStore.currentLanguage]);

  return (
    <ConfigProvider {...configs} >
      {children}
    </ConfigProvider>
  );
}

function App() {
  const userStore = createStore(UserStore);
  const i18nStore = createStore(I18nStore);
  const navStore = createStore(NavStore);

  return (
    <StoreProvider stores={[userStore, i18nStore, navStore]}>
      <AntdConfigProvider>
        <Suspense fallback={<PageLoading />}>
          <Router primary={false} >
            <TopLevelRouteSelector path="*" />
          </Router>
        </Suspense>
      </AntdConfigProvider>

    </StoreProvider>
  );
}

export default App;

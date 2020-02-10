import React, { useRef, useState, Suspense } from 'react';
import { Router } from "@reach/router";
import { HomePage } from './pages/Home';
import { StoreProvider, createStore } from 'simstate';
import { UserStore } from './stores/UserStore';
import { I18nStore } from "./i18n";
import PageLoading from "./components/PageLoading";
import { NavStore } from "./layouts/nav/NavStore";

const AsyncNormalPages = React.lazy(() => import("./pages/NormalPages"));


function App() {
  const userStore = createStore(UserStore);
  const i18nStore = createStore(I18nStore);
  const navStore = createStore(NavStore);

  return (
    <StoreProvider stores={[userStore, i18nStore, navStore]}>
      <Suspense fallback={<PageLoading />}>
        <Router primary={false}>
          <HomePage path="/" />
          <AsyncNormalPages path="/*" />
        </Router>
      </Suspense>

    </StoreProvider>
  );
}

export default App;

import React, { useRef, useState } from 'react';
import { Router } from "@reach/router";
import { HomePage } from './pages/Home';
import { StoreProvider, createStore } from 'simstate';
import { UserStore } from './stores/UserStore';
import RootLayout from './layouts/RootLayout';
import { I18nStore } from "./i18n";

function App() {
  const userStore = createStore(UserStore);
  const i18nStore = createStore(I18nStore);

  return (
    <StoreProvider stores={[userStore, i18nStore]}>

      <Router primary={false}>
        <HomePage path="/" />
        {/* <AsyncExplorePage path="/explore" />
          <AsyncPaperDetailPage path="/papers/:paperId" />
          <AsyncPaperEditPage path="/papers/:paperId/edit" />
          <AsyncPaperUploadPage path="/upload" />
          <AsyncProfilePage path="/profile" /> */}
      </Router>

    </StoreProvider>
  );
}

export default App;

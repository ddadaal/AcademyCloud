import React from "react";
import { RouteComponentProps, Redirect, Router } from "@reach/router";
import { UserStore } from "src/stores/UserStore";
import { useStore } from "simstate";
import { HomePageLayout } from "src/pages/Home/HomePageLayout";
import { LoginForm } from "src/pages/Home/Login";
import { RegisterForm } from "src/pages/Home/Register";
import { isResourcesDisabled } from "src/models/Scope";

const HomePage: React.FC<RouteComponentProps> = () => {
  const userStore = useStore(UserStore);
  if (userStore.loggedIn) {
    const { scope } = userStore.user!;

    return <Redirect noThrow={true} to={isResourcesDisabled(scope) ? "/expenses" : "/resources"} />;
  }

  return (
    <HomePageLayout>
      <Router>
        <LoginForm path="/" />
        <RegisterForm path="/register" />
      </Router>
    </HomePageLayout>
  );
}

export default HomePage;

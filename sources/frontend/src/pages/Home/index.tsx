import React from "react";
import { RouteComponentProps, Redirect, Router } from "@reach/router";
import { UserStore } from "src/stores/UserStore";
import { useStore } from "simstate";
import { HomePageLayout } from "src/pages/Home/HomePageLayout";
import { LoginForm } from "src/pages/Home/Login";
import { RegisterForm } from "src/pages/Home/Register";

const HomePage: React.FC<RouteComponentProps> = (props) => {
  const userStore = useStore(UserStore);
  if (userStore.loggedIn) {
    return <Redirect noThrow={true} to="/resources" />;
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

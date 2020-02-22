import React, { useMemo } from "react";
import { NavItemProps } from "src/layouts/nav/NavItemProps";
import { Scope } from "src/models/Scope";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { Router, RouteComponentProps, Redirect } from "@reach/router";
import { useSidenavs } from "src/pages/common/useSidenavs";
import { pathEquals } from "src/utils/path";
import { ErrorPage } from "src/pages/common/ErrorPage";

export interface IndexRoute extends NavItemProps {
  checkScope: (scope: Scope) => boolean;
  Component: React.ComponentType<RouteComponentProps>;
}

export function indexRoutes(routes: IndexRoute[], basePath: string) {

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  return (props: RouteComponentProps) => {


    const userStore = useStore(UserStore);

    const { scope } = userStore.user!!;

    const filteredSidenavs = useMemo(() => (
      routes.filter((x) => x.checkScope(scope))
    ), [scope]);

    useSidenavs(filteredSidenavs, basePath);

    if (pathEquals(props.location!!.pathname, basePath)) {
      return <Redirect noThrow={true} to={filteredSidenavs[0].path} />;
    }


    return (
      <Router>
        {filteredSidenavs.map(({ Component, path }) => (
          <Component key={path} path={path} />
        ))}
        <ErrorPage path="*" />
      </Router>
    );
  }
}


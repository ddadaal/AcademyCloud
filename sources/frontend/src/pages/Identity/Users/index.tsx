import React from "react";
import { RouteComponentProps } from "@reach/router";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { Localized, lang } from "src/i18n";
import { isSocialScope, isSystemScope, isProjectAdmin, isProjectScope, isDomainAdmin, isAdmin } from "src/models/Scope";
import { ProjectUsersTable } from "src/pages/Identity/Users/ProjectUsersTable";
import { useRefreshToken } from "src/utils/refreshToken";
import { SystemUsersTable } from "src/pages/Identity/Users/SystemUsersTable";
import { DomainUsersTable } from "src/pages/Identity/Users/DomainUsersTable";

const root = lang.identity.users;

export default function UsersPage(_: RouteComponentProps) {

  const [refreshToken, update] = useRefreshToken();

  const userStore = useStore(UserStore);

  if (!userStore.user) { return <div />; }

  const scope = userStore.user.scope;

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText><Localized id={root.title} /></TitleText>
        <a onClick={update}><Localized id={root.refresh} /></a>
      </TitleBar>
      {
        (!isSocialScope(scope) && isProjectScope(scope))
          ? <ProjectUsersTable
            refreshToken={refreshToken}
            isAdmin={isAdmin(scope)}
            projectId={scope.projectId!!}
          />
          : (
            isSystemScope(scope)
              ? <SystemUsersTable refreshToken={refreshToken} />
              : <DomainUsersTable refreshToken={refreshToken} domainId={scope.domainId} isAdmin={isAdmin(scope)} />
          )
      }
    </div>
  );
}


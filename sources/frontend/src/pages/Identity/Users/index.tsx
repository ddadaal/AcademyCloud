import React from "react";
import { RouteComponentProps } from "@reach/router";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { Localized, lang } from "src/i18n";
import { isSocialScope } from "src/models/Scope";
import { ProjectUsersTable } from "src/pages/Identity/Users/ProjectUsersTable";
import { SystemAndDomainUsersTable } from "src/pages/Identity/Users/SystemAndDomainUsersTable";

const root = lang.identity.users;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default function UsersPage(_: RouteComponentProps) {
  const userStore = useStore(UserStore);

  if (!userStore.user) { return <div />; }

  const scope = userStore.user.scope;

  const isProject = (!isSocialScope(scope) && (scope.projectId));

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText><Localized id={root.title} /></TitleText>
      </TitleBar>
      {
        isProject
          ? <ProjectUsersTable />
          : <SystemAndDomainUsersTable scope={scope} />
      }
    </div>
  );
}


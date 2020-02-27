import React from "react";
import { RouteComponentProps } from "@reach/router";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { Localized, lang } from "src/i18n";
import { ProjectsTable } from "src/pages/Identity/Projects/ProjectsTable";
import { useRefreshToken } from "src/utils/refreshToken";
import { CreateProjectButton } from "src/pages/Identity/Projects/CreateProjectButton";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";

const root = lang.identity.projects;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default function ProjectsPage(_: RouteComponentProps) {

  const userStore = useStore(UserStore);
  const [token, refresh] = useRefreshToken();

  if (!userStore.user) { return <div />; }

  const scope = userStore.user.scope;

  const isDomainAdmin = (!scope.projectId) && (scope.role === "admin");

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText><Localized id={root.title} /></TitleText>
        {isDomainAdmin
          ? <CreateProjectButton reload={refresh} />
          : null
        }
      </TitleBar>
      <ProjectsTable refreshToken={token} scope={scope} />
    </div>
  );
}



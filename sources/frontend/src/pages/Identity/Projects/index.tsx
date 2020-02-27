import React from "react";
import { RouteComponentProps } from "@reach/router";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { Localized, lang } from "src/i18n";
import { ProjectsTable } from "src/pages/Identity/Projects/ProjectsTable";
import { useRefreshToken } from "src/utils/refreshToken";

const root = lang.identity.projects;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default function ProjectsPage(_: RouteComponentProps) {
  const [token, refresh] = useRefreshToken();
  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText><Localized id={root.title} /></TitleText>
      </TitleBar>
      <ProjectsTable refreshToken={token} />
    </div>
  );
}



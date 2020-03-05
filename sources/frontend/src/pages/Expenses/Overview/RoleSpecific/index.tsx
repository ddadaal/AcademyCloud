import React from "react";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { isSystemScope, isDomainAdmin, isSocialScope, isProjectAdmin } from "src/models/Scope";
import { System } from "./System";
import { ProjectAdmin } from "src/pages/Expenses/Overview/RoleSpecific/ProjectAdmin";
import { DomainAdmin } from "src/pages/Expenses/Overview/RoleSpecific/DomainAdmin";

export const RoleSpecific: React.FC = () => {

  const { user } = useStore(UserStore);

  if (!user) { return <div />; }

  const { scope } = user;


  if (isSystemScope(scope)) {
    return <System />;
  }

  if (isDomainAdmin(scope)) {
    return <DomainAdmin domainId={scope.domainId} domainName={scope.domainName} />;
  }

  if (!isSocialScope(scope) && isProjectAdmin(scope)) {
    return <ProjectAdmin projectId={scope.projectId!!} projectName={scope.projectName!!} />;
  }

  return <div />;

}

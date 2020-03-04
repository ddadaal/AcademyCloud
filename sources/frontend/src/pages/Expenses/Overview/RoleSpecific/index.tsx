import React from "react";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { isSystemScope, isDomainAdmin } from "src/models/Scope";
import { System } from "./System";

export const RoleSpecific: React.FC = () => {

  const { user } = useStore(UserStore);

  if (!user) { return <div/>;}


  if (isSystemScope(user.scope)) {
    return <System />;
  }
  return <div/>;

}

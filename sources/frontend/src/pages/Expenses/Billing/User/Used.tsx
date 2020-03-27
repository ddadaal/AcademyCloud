import React, { useCallback } from "react";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { StatsPage } from "src/components/billings/StatsPage";
import { BillType, BillSubjectType } from "src/models/Billings";


export const ProjectUsed: React.FC = (props) => {

  const userStore = useStore(UserStore);

  const { scope, username } = userStore.user!!;

  return (
    <StatsPage
      billType={BillType.Used}
      billSubjectType={BillSubjectType.UserProjectAssignment}
      id={scope.userProjectAssignmentId!}
      name={username}
    />
  )
}

export default ProjectUsed;

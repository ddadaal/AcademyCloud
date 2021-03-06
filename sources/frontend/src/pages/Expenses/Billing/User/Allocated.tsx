import React, { useCallback } from "react";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { StatsPage } from "src/components/billings/StatsPage";
import { BillType, BillSubjectType } from "src/models/Billings";


export const UserAllocated: React.FC = (props) => {

  const userStore = useStore(UserStore);

  const { scope, username } = userStore.user!!;

  return (
    <StatsPage
      billType={BillType.Allocated}
      billSubjectType={BillSubjectType.UserProjectAssignment}
      id={scope.userProjectAssignmentId!}
      name={username}
    />
  )
}

export default UserAllocated;

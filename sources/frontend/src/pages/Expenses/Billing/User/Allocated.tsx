import React, { useCallback } from "react";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { StatsPage } from "src/components/billings/StatsPage";
import { BillType, BillSubjectType } from "src/models/Billings";


export const UserAllocated: React.FC = (props) => {

  const userStore = useStore(UserStore);

  const { userId, username } = userStore.user!!;

  return (
    <StatsPage
      billType={BillType.Allocated}
      billSubjectType={BillSubjectType.user}
      id={userId}
      name={username}
    />
  )
}

export default UserAllocated;

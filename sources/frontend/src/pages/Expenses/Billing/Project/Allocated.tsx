import React, { useCallback } from "react";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { StatsPage } from "src/components/billings/StatsPage";
import { BillType, BillSubjectType } from "src/models/Billings";


export const ProjectAllocated: React.FC = (props) => {

  const userStore = useStore(UserStore);

  const { projectId, projectName } = userStore.user!!.scope;

  return (
    <StatsPage
      billType={BillType.Allocated}
      billSubjectType={BillSubjectType.project}
      id={projectId!!}
      name={projectName!!}
    />
  )
}

export default ProjectAllocated;

import React, { useCallback } from "react";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { StatsPage } from "src/components/billings/StatsPage";
import { BillType, BillSubjectType } from "src/models/Billings";


export const DomainAllocated: React.FC = (props) => {

  const userStore = useStore(UserStore);

  const { domainId, domainName } = userStore.user!!.scope;

  return (
    <StatsPage
      billType={BillType.Allocated}
      billSubjectType={BillSubjectType.domain}
      id={domainId}
      name={domainName}
    />
  )
}

export default DomainAllocated;

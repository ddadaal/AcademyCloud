import React, { useMemo } from "react";
import { useAsync } from "react-async";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { CurrentAllocatedTable } from "src/components/billings/CurrentAllocatedTable";

const service = getApiService(BillingService);

const getDomains = () => service.getDomainsCurrentAllocatedBilling().then(x => x.billings);

interface Props {
  refreshToken: any;
}

export const DomainsAllocatedTable: React.FC<Props> = ({ refreshToken }) => {
  const { data, isPending } = useAsync({ promiseFn: getDomains, watch: refreshToken });

  const processedData = useMemo(() => {
    return data?.map(x => ({ ...x, subjectLink: `./${x.subjectId}` }))
  }, [data])

  return (
    <CurrentAllocatedTable subjectType="domain" data={processedData} loading={isPending} />
  )
}

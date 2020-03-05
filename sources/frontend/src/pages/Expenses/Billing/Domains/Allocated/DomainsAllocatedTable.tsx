import React, { useMemo } from "react";
import { AllocatedTable } from "src/components/billings/AllocatedTable";
import { useAsync } from "react-async";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";

const service = getApiService(BillingService);

const getDomains = () => service.getDomainsAllocatedBilling().then(x => x.billings);

interface Props {
  refreshToken: any;
}

export const DomainsAllocatedTable: React.FC<Props> = ({ refreshToken }) => {
  const { data, isPending } = useAsync({ promiseFn: getDomains, watch: refreshToken });

  const processedData = useMemo(() => {
    return data?.map(x => ({ ...x, subjectLink: `./${x.subjectId}`  }))
  }, [data])

  return (
    <AllocatedTable subjectType="domain" data={processedData} loading={isPending} />
  )
}

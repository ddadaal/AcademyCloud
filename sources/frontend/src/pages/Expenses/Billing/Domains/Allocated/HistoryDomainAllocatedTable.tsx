import React, { useCallback } from "react";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { HistoryAllocatedTable } from "src/components/billings/HistoryAllocatedTable";
import { useAsync } from "react-async";

interface Props {
  domainId: string;
  refreshToken: any;
}

const service = getApiService(BillingService);

export const HistoryDomainAllocatedTable: React.FC<Props> = ({ domainId, refreshToken }) => {
  const promiseFn = useCallback(async () => {
    const resp = await service.getDomainHistoryAllocatedBillings(domainId);
    return resp.billings;
  }, [domainId]);

  const { data, isPending } = useAsync({ promiseFn, watch: refreshToken });

  return <HistoryAllocatedTable data={data} loading={isPending} />
}


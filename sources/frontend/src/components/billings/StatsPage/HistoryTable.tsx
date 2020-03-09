import React, { useCallback } from "react";
import { getApiService } from "src/apis";
import { BillingService } from "src/apis/expenses/BillingService";
import { HistoryBillingsTable } from "src/components/billings/HistoryBillingsTable";
import { useAsync } from "react-async";
import { BillSubjectType, BillType } from "src/models/Billings";

interface Props {
  refreshToken: any;
  billType: BillType;
  billSubjectType: BillSubjectType;
  id: string;
}

const service = getApiService(BillingService);

export const HistoryTable: React.FC<Props> = ({ billType, billSubjectType, id, refreshToken }) => {

  const promiseFn = useCallback(async () => {
    const resp = billType === BillType.Allocated
      ? service.getHistoryAllocatedBillings(billSubjectType, id)
      : service.getHistoryUsedBillings(billSubjectType, id);
    return (await resp).billings;
  }, [billType, billSubjectType, id]);

  const { data, isPending } = useAsync({ promiseFn, watch: refreshToken });

  return (
    <HistoryBillingsTable
      loading={isPending}
      type={billSubjectType === BillSubjectType.User ? BillType.Used : billType}
      data={data}
    />
  )
}

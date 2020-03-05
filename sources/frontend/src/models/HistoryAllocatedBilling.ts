import { Resources } from "src/models/Resources";

export interface HistoryAllocatedBilling {
  id: string;
  resources: Resources;
  payerId: string;
  payerName: string;
  amount: number;
  startTime: string;
  endTime: string;

}

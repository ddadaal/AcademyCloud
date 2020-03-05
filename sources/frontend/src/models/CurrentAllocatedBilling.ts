import { Resources } from './Resources';

export interface CurrentAllocatedBilling {
  subjectId: string;
  subjectName: string;
  resources: Resources;
  amount: number;
  payerId: string;
  payerName: string;
  nextDue: string;
}

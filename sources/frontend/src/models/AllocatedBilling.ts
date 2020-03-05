import { Resources } from './Resources';

export interface AllocatedBilling {
  subjectId: string;
  subjectName: string;
  resources: Resources;
  amount: number;
  payerId: string;
  payerName: string;
  nextDue: string;
}

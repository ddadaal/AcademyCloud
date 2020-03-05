import { Resources } from './Resources';

export interface CurrentUsageBilling {
  subjectId: string;
  subjectName: string;
  resources: Resources;
  amount: number;
  nextDue: string;
}

export interface CurrentAllocatedBilling extends CurrentUsageBilling {
  payerId: string;
  payerName: string;
}

export interface HistoryUsageBilling {
  id: string;
  resources: Resources;
  amount: number;
  startTime: string;
  endTime: string;
}

export interface HistoryAllocatedBilling extends HistoryUsageBilling {
  payerId: string;
  payerName: string;
}

export enum BillType {
  Allocated= "allocated",
  Used = "used",
}

export enum BillSubjectType {
  domain = "domain",
  project = "project",
  user = "user",
}

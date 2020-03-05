import { Resources } from './Resources';

export interface CurrentUsedBilling {
  subjectId: string;
  subjectName: string;
  resources: Resources;
  amount: number;
  nextDue: string;
}

export interface CurrentAllocatedBilling extends CurrentUsedBilling {
  payerId: string;
  payerName: string;
}

export interface HistoryUsedBilling {
  id: string;
  resources: Resources;
  amount: number;
  startTime: string;
  endTime: string;
}

export interface HistoryAllocatedBilling extends HistoryUsedBilling {
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

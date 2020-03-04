import { TransactionType } from "src/models/TransactionType";

export interface OrgTransaction {
  id: string;
  time: string;
  amount: number;
  payerId: string;
  payerName: string;
  receiverId: string;
  receiverName: string;
  type: TransactionType;
  info?: string;
}

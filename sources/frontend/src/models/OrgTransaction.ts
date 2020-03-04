import {  TransactionReason } from "src/models/TransactionReason";

export interface OrgTransaction {
  id: string;
  time: string;
  amount: number;
  payerId: string;
  payerName: string;
  receiverId: string;
  receiverName: string;
  reason: TransactionReason;
}

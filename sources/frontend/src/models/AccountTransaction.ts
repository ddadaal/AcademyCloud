import {  TransactionReason } from "src/models/TransactionReason";

export interface AccountTransaction {
  id: string;
  time: string;
  amount: number;
  reason: TransactionReason;
}

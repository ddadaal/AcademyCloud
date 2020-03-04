import { TransactionsResponse, TransactionsService } from "src/apis/expenses/TransactionsService";
import { AccountTransaction } from "src/models/AccountTransaction";
import { OrgTransaction } from "src/models/OrgTransaction";
import { TransactionType } from "src/models/TransactionReason";

export class TransactionsServiceMock extends TransactionsService {
  async getAccountTransactions(): Promise<TransactionsResponse<AccountTransaction>> {
    await this.delay();

    return {
      transactions: [
        { id: "2", time: "2020-03-04T06:41:12.836Z", amount: -10, reason: {type: TransactionType.ProjectManagementFee, info: "123 (123)"} },
        { id: "1", time: "2020-03-04T06:40:12.836Z", amount: 10, reason: {type: TransactionType.Charge }},
      ] as AccountTransaction[]
    };

  }

  // limit <= 0 means all data
  async getSystemTransactions(limit = 0): Promise<TransactionsResponse<OrgTransaction>> {
    await this.delay();
    return {
      transactions: [
        { id: "2", time: "2020-03-04T06:41:12.836Z", amount: -10, reason: {type: TransactionType.ProjectManagementFee, info: "123 (123)"}, payerId: "cjdid", payerName: "cjd", receiverId: "cjyid", receiverName: "cjy" },
        { id: "1", time: "2020-03-04T06:40:12.836Z", amount: 10, reason: {type: TransactionType.Charge}, payerId: "cjdid", payerName: "cjd", receiverId: "cjyid", receiverName: "cjy" },
      ]
    };
  }

  async getDomainTransactions(): Promise<TransactionsResponse<OrgTransaction>> {
    return await this.getSystemTransactions();
  }
}

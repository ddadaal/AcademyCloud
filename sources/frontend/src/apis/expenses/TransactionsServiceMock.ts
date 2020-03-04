import { TransactionsResponse, TransactionsService } from "src/apis/expenses/TransactionsService";
import { AccountTransaction } from "src/models/AccountTransaction";
import { SystemTransaction } from "src/models/SystemTransaction";

export class TransactionsServiceMock extends TransactionsService {
  async getAccountTransactions(): Promise<TransactionsResponse<AccountTransaction>> {
    await this.delay();

    return {
      transactions: [
      ]
    };

  }

  // limit <= 0 means all data
  async getSystemTransactions(limit = 0): Promise<TransactionsResponse<SystemTransaction>> {
    await this.delay();
    return {
      transactions: []
    };
  }
}

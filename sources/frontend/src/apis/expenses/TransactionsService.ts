import { HttpService, HttpMethod } from "src/apis/HttpService";
import { AccountTransaction } from "src/models/AccountTransaction";
import { SystemTransaction } from "src/models/SystemTransaction";

export interface TransactionsResponse<T> {
  transactions: T[];
}

export class TransactionsService extends HttpService {
  async getAccountTransactions(): Promise<TransactionsResponse<AccountTransaction>> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: "/expenses/transactions/account",
    });

    return resp as TransactionsResponse<AccountTransaction>;

  }

  // limit <= 0 means all data
  async getSystemTransactions(limit = 0): Promise<TransactionsResponse<SystemTransaction>> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: "/expenses/transactions/system",
      params: { limit }
    });

    return resp as TransactionsResponse<SystemTransaction>;
  }
}

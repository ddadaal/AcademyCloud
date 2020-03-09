import { HttpService, HttpMethod } from "src/apis/HttpService";
import { AccountTransaction } from "src/models/AccountTransaction";
import { OrgTransaction } from 'src/models/OrgTransaction';

export interface TransactionsResponse<T> {
  transactions: T[];
}


// limit <= 0 means all data
// all returns ordered (descend) transactions by date
export class TransactionsService extends HttpService {
  async getAccountTransactions(limit = 0): Promise<TransactionsResponse<AccountTransaction>> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: "/expenses/transactions/account",
      params: { limit },
    });

    return resp as TransactionsResponse<AccountTransaction>;

  }

  async getSystemTransactions(limit = 0): Promise<TransactionsResponse<OrgTransaction>> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: "/expenses/transactions/system",
      params: { limit }
    });

    return resp as TransactionsResponse<OrgTransaction>;
  }

  async getDomainTransactions(domainId: string, limit = 0): Promise<TransactionsResponse<OrgTransaction>> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/expenses/transactions/domain/${domainId}`,
      params: { limit }
    });

    return resp as TransactionsResponse<OrgTransaction>;
  }

  async getProjectTransactions(projectId: string, limit = 0): Promise<TransactionsResponse<OrgTransaction>> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/expenses/transactions/project/${projectId}`,
      params: { limit }
    });

    return resp as TransactionsResponse<OrgTransaction>;
  }
}

import { HttpService, HttpMethod } from "../HttpService";

export interface GetBalanceResponse {
  balance: number;
}

export interface ChargeResponse {
  balance: number;
}

export class OverviewService extends HttpService {
  async getBalance(): Promise<GetBalanceResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: "/expenses/balance",
    });

    return resp as GetBalanceResponse;
  }

  async charge(amount: number): Promise<ChargeResponse> {
    const resp = await this.fetch({
      method: HttpMethod.POST,
      path: "/expenses/balance",
      body: { amount }
    });

    return resp as ChargeResponse;
  }

}

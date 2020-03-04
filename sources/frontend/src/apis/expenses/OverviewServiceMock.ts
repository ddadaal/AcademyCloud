import { OverviewService, GetBalanceResponse, ChargeResponse } from './OverviewService';

export class OverviewServiceMock extends OverviewService {
  async getBalance(): Promise<GetBalanceResponse> {
    await this.delay();
    return { balance: 500.00 };
  }

  async charge(amount: number): Promise<ChargeResponse> {
    await this.delay();
    return { balance: 500.00 + amount };
  }

}

import { BalanceService, GetBalanceResponse, ChargeResponse } from './BalanceService';

export class BalanceServiceMock extends BalanceService {
  async getBalance(): Promise<GetBalanceResponse> {
    await this.delay();
    return { balance: 500.00 };
  }

  async charge(amount: number): Promise<ChargeResponse> {
    await this.delay();
    return { balance: 500.00 + amount };
  }

}

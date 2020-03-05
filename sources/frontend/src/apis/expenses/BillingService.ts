import { HttpService, HttpMethod } from '../HttpService';
import { AllocatedBilling } from 'src/models/AllocatedBilling';

export interface BillingResponse<T> {
  billings: T[];
}

export type AllocatedBillingResponse = BillingResponse<AllocatedBilling>;

export class BillingService extends HttpService {
  async getDomainsAllocatedBilling(): Promise<AllocatedBillingResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: "/billings/domains/allocated",
    });

    return resp as AllocatedBillingResponse;
  }

}

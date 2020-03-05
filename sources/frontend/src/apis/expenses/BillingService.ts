import { HttpService, HttpMethod } from '../HttpService';
import { CurrentAllocatedBilling, HistoryAllocatedBilling } from "src/models/Billings";

export interface BillingResponse<T> {
  billings: T[];
}

export type CurrentAllocatedBillingResponse = BillingResponse<CurrentAllocatedBilling>;

export type HistoryAllocatedBillingsResponse = BillingResponse<HistoryAllocatedBilling>;

export class BillingService extends HttpService {
  async getDomainsCurrentAllocatedBilling(): Promise<CurrentAllocatedBillingResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: "/billings/domains/allocated",
    });

    return resp as CurrentAllocatedBillingResponse;
  }

  async getDomainHistoryAllocatedBillings(domainId: string): Promise<HistoryAllocatedBillingsResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/billings/domains/allocated/${domainId}`,
    });

    return resp as HistoryAllocatedBillingsResponse;
  }
}

import { HttpService, HttpMethod } from '../HttpService';
import { CurrentAllocatedBilling, HistoryAllocatedBilling, BillSubjectType, CurrentUsedBilling, HistoryUsedBilling } from "src/models/Billings";

export interface BillingResponse<T> {
  billings: T[];
}

export type CurrentAllocatedBillingResponse = BillingResponse<CurrentAllocatedBilling>;
export type CurrentUsedBillingResponse = BillingResponse<CurrentUsedBilling>;

export type HistoryAllocatedBillingsResponse = BillingResponse<HistoryAllocatedBilling>;
export type HistoryUsedBillingsResponse = BillingResponse<HistoryUsedBilling>;

export class BillingService extends HttpService {
  async getCurrentAllocatedBillings(subjectType: BillSubjectType): Promise<CurrentAllocatedBillingResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/billings/allocated/${subjectType}`,
    });

    return resp as CurrentAllocatedBillingResponse;
  }

  async getCurrentUsedBillings(subjectType: BillSubjectType): Promise<CurrentUsedBillingResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/billings/used/${subjectType}`,
    });

    return resp as CurrentUsedBillingResponse;
  }

  async getHistoryAllocatedBillings(subjectType: BillSubjectType, id: string): Promise<HistoryAllocatedBillingsResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/billings/allocated/${subjectType}/${id}`,
    });

    return resp as HistoryAllocatedBillingsResponse;
  }

  async getHistoryUsedBillings(subjectType: BillSubjectType, id: string): Promise<HistoryUsedBillingsResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/billings/used/${subjectType}/${id}`,
    });

    return resp as HistoryUsedBillingsResponse;
  }
}

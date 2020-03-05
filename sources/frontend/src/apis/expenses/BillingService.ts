import { HttpService, HttpMethod } from '../HttpService';
import { CurrentAllocatedBilling, HistoryAllocatedBilling, BillSubjectType, CurrentUsedBilling, HistoryUsedBilling } from "src/models/Billings";

export interface BillingsResponse<T> {
  billings: T[];
}

export interface BillingResponse<T> {
  billing: T;
}

export type CurrentAllocatedBillingResponse = BillingsResponse<CurrentAllocatedBilling>;
export type CurrentUsedBillingResponse = BillingsResponse<CurrentUsedBilling>;

export type HistoryAllocatedBillingsResponse = BillingsResponse<HistoryAllocatedBilling>;
export type HistoryUsedBillingsResponse = BillingsResponse<HistoryUsedBilling>;

export class BillingService extends HttpService {
  async getCurrentAllocatedBillings(subjectType: BillSubjectType): Promise<CurrentAllocatedBillingResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/billings/allocated/current/${subjectType}`,
    });

    return resp as CurrentAllocatedBillingResponse;
  }

  async getCurrentAllocatedBilling(subjectType: BillSubjectType, id: string): Promise<BillingResponse<CurrentAllocatedBilling>> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/billings/allocated/current/${subjectType}/${id}`
    });

    return resp as BillingResponse<CurrentAllocatedBilling>;
  }

  async getCurrentUsedBillings(subjectType: BillSubjectType): Promise<CurrentUsedBillingResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/billings/used/current/${subjectType}`,
    });

    return resp as CurrentUsedBillingResponse;
  }

  async getCurrentUsedBilling(subjectType: BillSubjectType, id: string): Promise<BillingResponse<CurrentUsedBilling>> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/billings/used/current/${subjectType}/${id}`
    });

    return resp as BillingResponse<CurrentUsedBilling>;
  }

  async getHistoryAllocatedBillings(subjectType: BillSubjectType, id: string): Promise<HistoryAllocatedBillingsResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/billings/allocated/history/${subjectType}/${id}`,
    });

    return resp as HistoryAllocatedBillingsResponse;
  }

  async getHistoryUsedBillings(subjectType: BillSubjectType, id: string): Promise<HistoryUsedBillingsResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `/billings/used/history/${subjectType}/${id}`,
    });

    return resp as HistoryUsedBillingsResponse;
  }
}

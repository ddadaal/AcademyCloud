import { BillingService, CurrentAllocatedBillingResponse, HistoryAllocatedBillingsResponse, CurrentUsedBillingResponse, HistoryUsedBillingsResponse, BillingResponse } from './BillingService';
import { BillSubjectType, CurrentAllocatedBilling } from "src/models/Billings";

const dummyResources = { cpu: 4, memory: 128, storage: 256 };
const dummyDate = "2020-03-05T03:15:17.484Z";

export class BillingServiceMock extends BillingService {
  async getCurrentAllocatedBillings(): Promise<CurrentAllocatedBillingResponse> {
    await this.delay();
    return {
      billings: [
        { subjectId: "NJUID", subjectName: "NJU", resources: dummyResources, payerId: "67id", payerName: "67Name", amount: 120, nextDue: dummyDate },
        { subjectId: "PKUID", subjectName: "PKU", resources: dummyResources, payerId: "fcid", payerName: "fcName", amount: 160, nextDue: dummyDate },
      ]
    };
  }


  async getCurrentAllocatedBilling(subjectType: BillSubjectType, id: string): Promise<BillingResponse<CurrentAllocatedBilling>> {
    await this.delay();

    return {
      billing:
        { subjectId: "NJUID", subjectName: "NJU", resources: dummyResources, payerId: "67id", payerName: "67Name", amount: 120, nextDue: dummyDate },
    }
  }

  async getCurrentUsedBillings(): Promise<CurrentUsedBillingResponse> {
    await this.delay();
    return {
      billings: [
        { subjectId: "NJUID", subjectName: "NJU", resources: dummyResources, amount: 120, nextDue: dummyDate },
        { subjectId: "PKUID", subjectName: "PKU", resources: dummyResources, amount: 160, nextDue: dummyDate },
      ]
    };
  }

  async getHistoryAllocatedBillings(domainId: string): Promise<HistoryAllocatedBillingsResponse> {
    await this.delay();
    return {
      billings: [
        { id: "1", resources: dummyResources, payerId: "67id", payerName: "67Name", amount: 120, startTime: dummyDate, endTime: dummyDate },
        { id: "2", resources: dummyResources, payerId: "fcid", payerName: "fcName", amount: 160, startTime: dummyDate, endTime: dummyDate },
      ]
    };
  }

  async getHistoryUsedBillings(domainId: string): Promise<HistoryUsedBillingsResponse> {
    await this.delay();
    return {
      billings: [
        { id: "1", resources: dummyResources, amount: 120, startTime: dummyDate, endTime: dummyDate },
        { id: "2", resources: dummyResources, amount: 160, startTime: dummyDate, endTime: dummyDate },
      ]
    };
  }
}

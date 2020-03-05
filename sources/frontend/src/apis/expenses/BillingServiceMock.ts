import { BillingService, AllocatedBillingResponse } from './BillingService';

export class BillingServiceMock extends BillingService {
  async getDomainsAllocatedBilling(): Promise<AllocatedBillingResponse> {
    await this.delay();
    return {
      billings: [
        { subjectId: "NJUID", subjectName: "NJU", resources: { cpu: 4, memory: 128, storage: 256 }, payerId: "67id", payerName: "67Name", amount: 120, nextDue: "2020-03-05T03:15:17.484Z" },
        { subjectId: "PKUID", subjectName: "PKU", resources: { cpu: 4, memory: 128, storage: 256 }, payerId: "fcid", payerName: "fcName", amount: 160, nextDue: "2020-03-05T03:15:17.484Z" },
      ]
    };
  }
}

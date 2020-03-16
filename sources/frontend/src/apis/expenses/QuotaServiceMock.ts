import { QuotaService, QuotaStatus } from './QuotaService';

export class QuotaServiceMock extends QuotaService {
  async getQuotaStatus(): Promise<QuotaStatus> {
    await this.delay();

    return {
      used: { cpu: 1, memory: 2, storage: 3 },
      total: { cpu: 2, memory: 4, storage: 6 }
    };
  }
}

import { Resources } from 'src/models/Resources';
import { HttpService, HttpMethod } from '../HttpService';
import { BillSubjectType } from 'src/models/Billings';

export interface QuotaStatus {
  used: Resources;
  total: Resources;
}

export class QuotaService extends HttpService {
  async getQuotaStatus(type: BillSubjectType, id: string): Promise<QuotaStatus> {
    return await this.fetch({
      method: HttpMethod.GET,
      path: `/expenses/quotaStatus/${type}/${id}`,
    });
  }

}

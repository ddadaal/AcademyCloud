import { HttpService, HttpMethod } from "../HttpService";
import { Domain } from "src/models/Domain";
import { Resources } from 'src/models/Resources';

export interface GetDomainsResponse {
  domains: Domain[];
}

export class DomainsService extends HttpService {
  async getDomains(): Promise<GetDomainsResponse> {
    const resp = await this.fetch<GetDomainsResponse>({
      method: HttpMethod.GET,
      path: "/identity/domains",
    });

    return resp;
  }

  async setResources(domainId: string, resources: Resources): Promise<void> {
    await this.fetch({
      method: HttpMethod.PATCH,
      path: `/identity/domains/${domainId}/resources`,
      body: { resources },
    });
  }

  async setAdmins(domainId: string, adminIds: string[]): Promise<void> {
    await this.fetch({
      method: HttpMethod.PATCH,
      path: `/identity/domains/${domainId}/admins`,
      body: { ids: adminIds },
    });
  }

  async setPayUser(domainId: string, payUserId: string): Promise<void> {
    await this.fetch({
      method: HttpMethod.PATCH,
      path: `/identity/domains/${domainId}/payUser`,
      body: { payUserId },
    });
  }

  async createDomain(name: string, payUserId: string): Promise<void> {
    await this.fetch({
      method: HttpMethod.POST,
      path: `/identity/domains`,
      body: { name, payUserId },
    });

  }

  // 400, { code:  }
  async deleteDomain(domainId: string): Promise<void> {
    await this.fetch({
      method: HttpMethod.DELETE,
      path: `/identity/domains/${domainId}`,
    });
  }
}

import { HttpService, HttpMethod } from "../HttpService";
import { Domain } from "src/models/Domain";
import { Resources } from 'src/models/Resources';
import { User } from "src/models/User";
import { UserRole } from "src/models/Scope";

export interface GetDomainsResponse {
  domains: Domain[];
}

export interface GetUsersOfDomainResponse {
  admins: User[];
  members: User[];
  payUser: User;
}

export class DomainsService extends HttpService {
  async getDomains(): Promise<GetDomainsResponse> {
    const resp = await this.fetch<GetDomainsResponse>({
      method: HttpMethod.GET,
      path: "/identity/domains",
    });

    return resp;
  }

  async getUsersOfDomain(domainId: string): Promise<GetUsersOfDomainResponse> {
    const resp = await this.fetch<GetUsersOfDomainResponse>({
      method: HttpMethod.GET,
      path: `/identity/domains/${domainId}/users`,
    });

    return resp;
  }

  async addUserToDomain(domainId: string, userId: string, role: UserRole): Promise<void> {
    await this.fetch({
      method: HttpMethod.POST,
      path: `/identity/domains/${domainId}/users`,
      body: { userId, role },
    });
  }

  // error: status code 400, { code: "payUser" | "onlyAdmin" }
  async changeUserRole(domainId: string, userId: string, role: UserRole): Promise<void> {
    await this.fetch({
      method: HttpMethod.PATCH,
      path: `/identity/domains/${domainId}/users/${userId}`,
      body: { role },
    });
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

  // 400, { code: "payUser" | "onlyAdmin" }
  async removeUserFromDomain(domainId: string, userId: string): Promise<void> {
    await this.fetch({
      method: HttpMethod.DELETE,
      path: `/identity/domains/${domainId}/users/${userId}`,
    });
  }


}

import { AccountService, ProfileResponse, GetJoinedDomainsResponse, GetJoinableDomainsResponse, GetScopesResponse } from './AccountService';
import { makeHttpError } from '../HttpService';
import { UserRole } from "src/models/Scope";

export class AccountServiceMock extends AccountService {

  async getScopes(): Promise<GetScopesResponse> {
    await this.delay();

    return {
      scopes: [
        { domainId: "NJUID", domainName: "NJU", role: UserRole.Member },
        { domainId: "NJUID", domainName: "NJU", projectName: "67", projectId: "67", role: UserRole.Admin },
        { domainId: "NJUID", domainName: "NJU", projectName: "fgh", projectId: "fgh", role: UserRole.Member },
        { domainId: "PKUID", domainName: "PKU", role: UserRole.Admin },
        { social: true, domainId: "socialID", domainName: "social", projectName: "test", projectId: "test", role: UserRole.Admin },
      ],
    }
  }


  async getProfile(): Promise<ProfileResponse> {
    await this.delay();
    return {
      profile: {
        id: "e10fcbe0-443e-440f-ba88-2a3dde2f534a",
        username: "test",
        email: "ddadaal@outlook.com",
        name: "Chen Junda",
      }
    }
  }

  async updateProfile(email: string, name: string): Promise<ProfileResponse> {
    await this.delay();
    return {
      profile: {
        id: "e10fcbe0-443e-440f-ba88-2a3dde2f534a",
        username: "test",
        email: email ?? "ddadaal@outlook.com",
        name: name,
      }
    }
  }

  async updatePassword(password: string): Promise<void> {
    await this.delay();
  }

  async getJoinedDomains(): Promise<GetJoinedDomainsResponse> {
    await this.delay();
    return {
      domains: [
        { domainId: "NJUID", domainName: "NJU", role: UserRole.Member },
        { domainId: "PKUID", domainName: "PKU", role: UserRole.Admin },
      ]
    }
  }

  async exitDomain(domainId: string): Promise<void> {
    throw makeHttpError(403, { reason: "isPayAccount" });
  }

  async getJoinableDomains(): Promise<GetJoinableDomainsResponse> {
    await this.delay();
    return {
      domains: [
        { id: "NJUID", name: "NJU" },
        { id: "PKUID", name: "PKU" },
      ]
    };
  }

  async joinDomain(domainId: string): Promise<void> {
    await this.delay();
  }
}

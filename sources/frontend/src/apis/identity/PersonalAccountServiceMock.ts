import { PersonalAccountService, ProfileResponse, UpdateProfileRequest, GetDomainsResponse } from './PersonalAccountService';
import { makeHttpError } from '../HttpService';

export class PersonalAccountServiceMock extends PersonalAccountService {
  async getProfile(): Promise<ProfileResponse> {
    await this.delay();
    return {
      profile: {
        id: "e10fcbe0-443e-440f-ba88-2a3dde2f534a",
        username: "test",
        email: "ddadaal@outlook.com",
      }
    }
  }

  async updateProfile(request: UpdateProfileRequest): Promise<ProfileResponse> {
    await this.delay();
    return {
      profile: {
        id: "e10fcbe0-443e-440f-ba88-2a3dde2f534a",
        username: "test",
        email: request.email ?? "ddadaal@outlook.com",
      }
    }
  }

  async updatePassword(password: string): Promise<void> {
    await this.delay();
  }

  async getDomains(): Promise<GetDomainsResponse> {
    await this.delay();
    return {
      domains: [
        { domainId: "NJUID", domainName: "NJU", role: "member" },
        { domainId: "PKUID", domainName: "PKU", role: "admin" },
      ]
    }
  }

  async exitDomain(domainId: string): Promise<void> {
    throw makeHttpError(403, { reason: "isPayAccount" });
  }
}

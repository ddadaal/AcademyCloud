import { PersonalAccountService, ProfileResponse, UpdateProfileRequest, GetDomainsResponse } from './PersonalAccountService';
import { delay } from "src/utils/delay";
import { HttpMethod } from "src/apis/HttpService";

export class PersonalAccountServiceMock extends PersonalAccountService {
  async getProfile(): Promise<ProfileResponse> {
    await delay(1000);
    return {
      profile: {
        id: "e10fcbe0-443e-440f-ba88-2a3dde2f534a",
        username: "test",
        email: "ddadaal@outlook.com",
      }
    }
  }

  async updateProfile(request: UpdateProfileRequest): Promise<ProfileResponse> {
    await delay(1000);
    return {
      profile: {
        id: "e10fcbe0-443e-440f-ba88-2a3dde2f534a",
        username: "test",
        email: request.email ?? "ddadaal@outlook.com",
      }
    }
  }

  async updatePassword(password: string): Promise<void> {
    await delay(1000);
  }

  async getDomains(): Promise<GetDomainsResponse> {
    return {
      domains: [
        { domainId: "NJUID", domainName: "NJU", role: "member" },
        { domainId: "PKUID", domainName: "PKU", role: "admin" },
      ]
    }
  }
}

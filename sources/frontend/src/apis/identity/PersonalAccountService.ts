import { HttpService, HttpMethod } from '../HttpService';
import { Profile } from "src/models/Profile";
import { UserDomainAssignment } from "src/models/UserDomainAssignment";

export interface ProfileResponse {
  profile: Profile;
}

export interface UpdateProfileRequest {
  email?: string;
}

export interface GetDomainsResponse {
  domains: UserDomainAssignment[];
}

export interface ExitDomainsError {
  reason: "isPayAccount" | "notJoined";
}

export class PersonalAccountService extends HttpService {
  async getProfile(): Promise<ProfileResponse> {
    const response = await this.fetch<ProfileResponse>({
      method: HttpMethod.GET,
      path: "/identity/account/profile",
    });

    return response;
  }

  async updateProfile(request: UpdateProfileRequest): Promise<ProfileResponse> {
    const response = await this.fetch<ProfileResponse>({
      method: HttpMethod.PATCH,
      path: "/identity/account/profile",
      body: request,
    });

    return response;
  }

  async updatePassword(original: string, updated: string): Promise<void> {
    await this.fetch({
      method: HttpMethod.PATCH,
      path: "/identity/account/password",
      body: { original, updated }
    });
  }

  async getDomains(): Promise<GetDomainsResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: "/identity/account/domains",
    });

    return resp as GetDomainsResponse;
  }

  // if not successful, throw a ExitDomainsError.
  async exitDomain(domainId: string): Promise<void> {
    await this.fetch({
      method: HttpMethod.DELETE,
      path: `/identity/account/domains/${domainId}`,
    });
  }
}

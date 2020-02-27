import { HttpService, HttpMethod } from '../HttpService';
import { Profile } from "src/models/Profile";
import { UserDomainAssignment } from "src/models/UserDomainAssignment";

export interface ProfileResponse {
  profile: Profile;
}

export interface UpdateProfileRequest {
  email?: string;
}

export interface GetJoinedDomainsResponse {
  domains: UserDomainAssignment[];
}

export interface ExitDomainsError {
  reason: "isPayAccount" | "notJoined";
}

export interface GetJoinableDomainsResponse {
  domains: { id: string; name: string }[];
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

  async getJoinedDomains(): Promise<GetJoinedDomainsResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: "/identity/account/joinedDomains",
    });

    return resp as GetJoinedDomainsResponse;
  }

  // if not successful, throw a ExitDomainsError.
  async exitDomain(domainId: string): Promise<void> {
    await this.fetch({
      method: HttpMethod.DELETE,
      path: `/identity/account/joinedDomains/${domainId}`,
    });
  }

  async getJoinableDomains(): Promise<GetJoinableDomainsResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: `identity/account/joinableDomains`,
    });

    return resp as GetJoinableDomainsResponse;
  }

  async joinDomain(domainId: string): Promise<void> {
    await this.fetch({
      method: HttpMethod.POST,
      path: `identity/account/joinableDomains/${domainId}`,
    });
  }
}

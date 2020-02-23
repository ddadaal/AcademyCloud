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
}

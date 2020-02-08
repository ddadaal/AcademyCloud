import { HttpService, HttpMethod } from "../HttpService";
import { ScopeableTarget, Scope } from "../../models/account";

export interface LoginResponse {
  token: string;
  scope: Scope;
}

export class AccountService extends HttpService {
  async getScopeableTargets(username: string, password: string, domainName: string): Promise<ScopeableTarget[]> {
    const data = await this.fetch({
      method: HttpMethod.GET,
      params: { username, password, domainName },
      path: "/account",
    });

    return data as ScopeableTarget[];
  }

  async login(username: string, password: string, domainName: string, projectName?: string): Promise<LoginResponse> {
    const data = await this.fetch<LoginResponse>({
      method: HttpMethod.POST,
      params: { username, password, domainName, projectName },
      path: "/account"
    });

    if (data.token) {
      this.setToken(data.token);
    }
    return data;
  }

  setToken(token: string) {
    if (token) {
      this.axios.defaults.headers.common.Authorization = `Bearer ${token}`;
    } else {
      delete this.axios.defaults.headers.common.Authorization;
    }
  }
}

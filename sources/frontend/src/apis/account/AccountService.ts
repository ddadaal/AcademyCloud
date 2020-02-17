import { HttpService, HttpMethod } from "../HttpService";
import { Scope } from "../../models/account";

export interface LoginResponse {
  token: string;
}

export class AccountService extends HttpService {
  async getScopes(username: string, password: string): Promise<Scope[]> {
    const data = await this.fetch({
      method: HttpMethod.GET,
      params: { username, password },
      path: "/account/scopes",
    });

    return data as Scope[];
  }

  async login(username: string, password: string, scope: Scope): Promise<LoginResponse> {
    const data = await this.fetch<LoginResponse>({
      method: HttpMethod.POST,
      body: { username, password, scope },
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

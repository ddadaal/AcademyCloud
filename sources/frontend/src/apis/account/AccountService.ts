import { HttpService, HttpMethod } from "../HttpService";
import { Scope } from 'src/models/Scope';

export interface LoginResponse {
  token: string;
}

export interface RegisterResponse {
  token: string;
  scope: Scope;
}

export interface ScopesResponse {
  scopes: Scope[];
  defaultScope?: Scope;
  lastLoginScope?: Scope;
}

export class AccountService extends HttpService {
  async getScopes(username: string, password: string): Promise<ScopesResponse> {
    const data = await this.fetch<ScopesResponse>({
      method: HttpMethod.GET,
      params: { username, password },
      path: "/auth/scopes",
    });

    return data;
  }

  async login(username: string, password: string, scope: Scope): Promise<LoginResponse> {
    const data = await this.fetch<LoginResponse>({
      method: HttpMethod.POST,
      body: { username, password, scope },
      path: "/auth/token"
    });

    if (data.token) {
      this.setToken(data.token);
    }
    return data;
  }

  async changeScope(scope: Scope): Promise<LoginResponse> {
    const data = await this.fetch<LoginResponse>({
      method: HttpMethod.POST,
      body: { scope },
      path: "/auth/token",
    });

    if (data.token) {
      this.setToken(data.token);
    }

    return data;
  }

  async register(username: string, password: string, email: string): Promise<RegisterResponse> {
    const data = await this.fetch<RegisterResponse>({
      method: HttpMethod.POST,
      body: { username, password, email },
      path: "/identity/account"
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

import { AccountService, LoginResponse, ScopesResponse, RegisterResponse } from "./AccountService";
import { Scope } from "src/models/account";
import { delay } from "src/utils/delay";
import { makeHttpError } from '../HttpService';

export class AccountServiceMock extends AccountService {
  async getScopes(username: string, password: string): Promise<ScopesResponse> {
    await delay(1000);
    if (username === "noscope") { return { scopes: [] }; }

    return {
      scopes: [
        { domainId: "NJU", domainName: "NJU", role: "member" },
        { domainId: "NJU", domainName: "NJU", projectName: "67", projectId: "67", role: "admin" },
      ],
      lastLoginScope: {
        domainId: "NJU", domainName: "NJU", role: "member",
      },
      defaultScope: {
        domainId: "NJU", domainName: "NJU", role: "member",
      }
    }
  };

  async login(username: string, password: string, scope: Scope): Promise<LoginResponse> {
    if (username === "401") {
      throw makeHttpError(401);
    }

    return {
      token: "testtoken",
    }
  }

  async register(username: string, password: string): Promise<RegisterResponse> {
    if (username === "403") {
      throw makeHttpError(403);
    }

    return { token: "testtoken", scope: { domainId: "social", domainName: "Social", projectId: username, projectName: username, role: "member" } }
  }

}

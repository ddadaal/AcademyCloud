import { AccountService, LoginResponse, ScopesResponse, RegisterResponse } from "./AccountService";
import { delay } from "src/utils/delay";
import { makeHttpError, HttpMethod } from '../HttpService';
import { Scope } from "src/models/Scope";

export class AccountServiceMock extends AccountService {
  async getScopes(username: string): Promise<ScopesResponse> {
    await this.delay();
    if (username === "system") {
      return {
        scopes: [{
          system: true,
          domainId: "system",
          domainName: "system",
          role: "admin",
        }]
      };
    }

    return {
      scopes: [
        { domainId: "NJU", domainName: "NJU", role: "member" },
        { domainId: "NJU", domainName: "NJU", projectName: "67", projectId: "67", role: "admin" },
        { domainId: "NJU", domainName: "NJU", projectName: "fgh", projectId: "fgh", role: "member" },
        { domainId: "PKU", domainName: "PKU", role: "admin" },
        { social: true, domainId: "social", domainName: "social", projectName: username, projectId: username, role: "admin" },
      ],
      lastLoginScope: {
        domainId: "NJU", domainName: "NJU", role: "member",
      },
      defaultScope: {
        domainId: "NJU", domainName: "NJU", role: "member",
      }
    }
  }

  async refreshScopes(): Promise<ScopesResponse> {
    await this.delay();
    return await this.getScopes("test");
  }

  async changeScope(scope: Scope): Promise<LoginResponse> {
    await this.delay();
    return { token: `testtoken${JSON.stringify(scope)}` };
  }

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

    return {
      token: "testtoken",
      scope: { domainId: "social", domainName: "Social", projectId: username, projectName: username, role: "member" }
    }
  }

}

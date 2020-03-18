import { AuthenticationService, LoginResponse, ScopesResponse, RegisterResponse } from "./AuthenticationService";
import { delay } from "src/utils/delay";
import { makeHttpError, HttpMethod } from '../HttpService';
import { Scope } from "src/models/Scope";

export class AuthenticationServiceMock extends AuthenticationService {
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
        { domainId: "NJUID", domainName: "NJU", role: "member" },
        { domainId: "NJUID", domainName: "NJU", projectName: "67", projectId: "67", role: "admin" },
        { domainId: "NJUID", domainName: "NJU", projectName: "fgh", projectId: "fgh", role: "member" },
        { domainId: "PKUID", domainName: "PKU", role: "admin" },
        { social: true, domainId: "socialID", domainName: "social", projectName: username, projectId: username, role: "admin" },
      ],
      lastLoginScope: {
        domainId: "NJUID", domainName: "NJU", role: "member",
      },
      defaultScope: {
        domainId: "NJUID", domainName: "NJU", role: "member",
      }
    }
  }

  async changeScope(scope: Scope): Promise<LoginResponse> {
    await this.delay();
    return {
      userId: "123", token: `testtoken${JSON.stringify(scope)}`,
      userActive: false , scopeActive: true,
    };
  }

  async login(username: string, password: string, scope: Scope): Promise<LoginResponse> {
    if (username === "401") {
      throw makeHttpError(401);
    }

    return {
      userId: "123",
      token: "testtoken",
      userActive: true,
      scopeActive: false,
    }
  }

  async register(username: string, password: string): Promise<RegisterResponse> {
    if (username === "403") {
      throw makeHttpError(403);
    }

    return {
      userId: "123",
      token: "testtoken",
      scope: { social: true, domainId: "social", domainName: "Social", projectId: username, projectName: username, role: "member" }
    }
  }

}

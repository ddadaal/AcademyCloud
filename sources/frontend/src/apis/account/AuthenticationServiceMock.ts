import { AuthenticationService, LoginResponse, ScopesResponse, RegisterResponse } from "./AuthenticationService";
import { delay } from "src/utils/delay";
import { makeHttpError, HttpMethod } from '../HttpService';
import { Scope, UserRole } from "src/models/Scope";

export class AuthenticationServiceMock extends AuthenticationService {
  async getScopes(username: string): Promise<ScopesResponse> {
    await this.delay();
    if (username === "system") {
      return {
        scopes: [{
          system: true,
          domainId: "system",
          domainName: "system",
          role: UserRole.Admin,
        }]
      };
    }

    return {
      scopes: [
        { domainId: "NJUID", domainName: "NJU", role: UserRole.Member },
        { domainId: "NJUID", domainName: "NJU", projectName: "67", projectId: "67", role: UserRole.Admin },
        { domainId: "NJUID", domainName: "NJU", projectName: "fgh", projectId: "fgh", role: UserRole.Member },
        { domainId: "PKUID", domainName: "PKU", role: UserRole.Admin },
        { social: true, domainId: "socialID", domainName: "social", projectName: username, projectId: username, role: UserRole.Admin },
      ],
      lastLoginScope: {
        domainId: "NJUID", domainName: "NJU", role: UserRole.Member,
      },
      defaultScope: {
        domainId: "NJUID", domainName: "NJU", role: UserRole.Member,
      }
    }
  }

  async changeScope(scope: Scope): Promise<LoginResponse> {
    await this.delay();
    return {
      userId: "123", token: `testtoken${JSON.stringify(scope)}`,
      userActive: true , scopeActive: true,
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
      scopeActive: true,
    }
  }

  async register(username: string, password: string): Promise<RegisterResponse> {
    if (username === "403") {
      throw makeHttpError(403);
    }

    return {
      userId: "123",
      token: "testtoken",
      scope: { social: true, domainId: "social", domainName: "Social", projectId: username, projectName: username, role: UserRole.Member }
    }
  }

}

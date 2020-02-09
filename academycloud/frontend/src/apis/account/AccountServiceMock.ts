import { AccountService, LoginResponse } from "./AccountService";
import { ScopeableTarget } from "src/models/account";

export class AccountServiceMock extends AccountService {
  async getScopeableTargets(username: string, password: string, domainName: string): Promise<ScopeableTarget[]> {
    if (username === "noscope") { return []; }

    return [
      { type: "project", name: "67", id: "67Project" },
      { type: "domain", name: "NJU", id: "NJUDomain" },
    ];

  }

  async login(username: string, password: string, domainName: string, projectName?: string): Promise<LoginResponse> {
    if (username === "noscope") { }
    if (username === "401") { }

    return {
      token: "testtoken",
      scope: {
        type: projectName ? "project" : "domain",
        name: projectName ?? domainName,
        id: projectName ? `${projectName}Id` : `${domainName}Id`,
        role: "admin",
      }
    }
  }

}

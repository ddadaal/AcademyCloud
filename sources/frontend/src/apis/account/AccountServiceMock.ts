import { AccountService, LoginResponse } from "./AccountService";
import { Scope } from "src/models/account";
import { delay } from "src/utils/delay";

export class AccountServiceMock extends AccountService {
  async getScopes(username: string, password: string): Promise<Scope[]> {
    await delay(1000);
    if (username === "noscope") { return []; }

    return [
      { domainId: "NJU", domainName: "NJU", role: "member" },
      { domainId: "NJU", domainName: "NJU", projectName: "67", projectId: "67", role: "admin" },
    ];

  }

  async login(username: string, password: string, scope: Scope): Promise<LoginResponse> {
    if (username === "noscope") { }
    if (username === "401") { }

    return {
      token: "testtoken",
    }
  }

}

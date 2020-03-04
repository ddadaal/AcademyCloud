import { HttpService } from "./HttpService";
import { AuthenticationService } from "./account/AuthenticationService";
import { AuthenticationServiceMock } from "./account/AuthenticationServiceMock";
import { InstanceService } from "./instance/InstanceService";
import { AccountService } from "src/apis/identity/AccountService";
import { AccountServiceMock } from "src/apis/identity/AccountServiceMock";
import { DomainsService } from "src/apis/identity/DomainsService";
import { DomainsServiceMock } from "src/apis/identity/DomainsServiceMock";
import { UsersService } from "./identity/UsersService";
import { UsersServiceMock } from "./identity/UsersServiceMock";
import { ProjectsService } from "src/apis/identity/ProjectsService";
import { ProjectsServiceMock } from "src/apis/identity/ProjectsServiceMock";
import { OverviewService } from "src/apis/expenses/OverviewService";
import { OverviewServiceMock } from "src/apis/expenses/OverviewServiceMock";
import { TransactionsService } from "src/apis/expenses/TransactionsService";
import { TransactionsServiceMock } from "src/apis/expenses/TransactionsServiceMock";

export const USE_MOCK = true;

const services = [
  [AuthenticationService, USE_MOCK ? AuthenticationServiceMock : AuthenticationService],
  [InstanceService, InstanceService],
  [AccountService, USE_MOCK ? AccountServiceMock : AccountService],
  [DomainsService, USE_MOCK ? DomainsServiceMock : DomainsService],
  [UsersService, USE_MOCK ? UsersServiceMock : UsersService],
  [ProjectsService, USE_MOCK ? ProjectsServiceMock : ProjectsService],
  [OverviewService, USE_MOCK ? OverviewServiceMock : OverviewService],
  [TransactionsService, USE_MOCK ? TransactionsServiceMock : TransactionsService],
] as const;


const serviceConfig = new Map<HttpServiceType, HttpService>();

services.forEach((item) => {
  serviceConfig.set(item[0], new item[1]());
});

export interface HttpServiceType {
  new(...args: unknown[]): HttpService;
}

export function getApiService<T extends HttpServiceType>(serviceType: T) {
  // Sure I know what services there are.
  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  return serviceConfig.get(serviceType)! as InstanceType<T>;
}

import { HttpService } from "./HttpService";
import { AuthenticationService } from "./account/AuthenticationService";
import { AuthenticationServiceMock } from "./account/AuthenticationServiceMock";
import { ResourcesService } from "./resources/ResourcesService";
import { AccountService } from "src/apis/identity/AccountService";
import { AccountServiceMock } from "src/apis/identity/AccountServiceMock";
import { DomainsService } from "src/apis/identity/DomainsService";
import { DomainsServiceMock } from "src/apis/identity/DomainsServiceMock";
import { UsersService } from "./identity/UsersService";
import { UsersServiceMock } from "./identity/UsersServiceMock";
import { ProjectsService } from "src/apis/identity/ProjectsService";
import { ProjectsServiceMock } from "src/apis/identity/ProjectsServiceMock";
import { BalanceService } from "src/apis/expenses/BalanceService";
import { BalanceServiceMock } from "src/apis/expenses/BalanceServiceMock";
import { TransactionsService } from "src/apis/expenses/TransactionsService";
import { TransactionsServiceMock } from "src/apis/expenses/TransactionsServiceMock";
import { BillingService } from './expenses/BillingService';
import { BillingServiceMock } from './expenses/BillingServiceMock';
import { ResourcesServiceMock } from "./resources/ResourcesServiceMock";
import { InstanceServiceMock } from './resources/InstanceServiceMock';
import { InstanceService } from "src/apis/resources/InstanceService";

export const USE_MOCK = true;

const services = [
  [AuthenticationService, USE_MOCK ? AuthenticationServiceMock : AuthenticationService],
  [ResourcesService, USE_MOCK ? ResourcesServiceMock : ResourcesService],
  [InstanceService, USE_MOCK ? InstanceServiceMock : InstanceService],
  [AccountService, USE_MOCK ? AccountServiceMock : AccountService],
  [DomainsService, USE_MOCK ? DomainsServiceMock : DomainsService],
  [UsersService, USE_MOCK ? UsersServiceMock : UsersService],
  [ProjectsService, USE_MOCK ? ProjectsServiceMock : ProjectsService],
  [BalanceService, USE_MOCK ? BalanceServiceMock : BalanceService],
  [TransactionsService, USE_MOCK ? TransactionsServiceMock : TransactionsService],
  [BillingService, USE_MOCK ? BillingServiceMock : BillingService],
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

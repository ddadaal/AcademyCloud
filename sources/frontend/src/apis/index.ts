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
import { QuotaService } from './expenses/QuotaService';
import { QuotaServiceMock } from './expenses/QuotaServiceMock';
import { VolumeService } from "src/apis/resources/VolumeService";
import { VolumeServiceMock } from "src/apis/resources/VolumeServiceMock";

export const USE_MOCK = true;

const services = [
  [AuthenticationService, AuthenticationServiceMock],
  [ResourcesService, ResourcesServiceMock],
  [InstanceService, InstanceServiceMock],
  [VolumeService, VolumeServiceMock],
  [AccountService, AccountServiceMock],
  [DomainsService, DomainsServiceMock],
  [UsersService, UsersServiceMock],
  [ProjectsService, ProjectsServiceMock],
  [QuotaService, QuotaServiceMock],
  [BalanceService, BalanceServiceMock],
  [TransactionsService, TransactionsServiceMock],
  [BillingService, BillingServiceMock],
] as const;


const serviceConfig = new Map<HttpServiceType, HttpService>();

services.forEach((item) => {
  serviceConfig.set(item[0], new item[USE_MOCK ? 1 : 0]());
});

export interface HttpServiceType {
  new(...args: unknown[]): HttpService;
}

export function getApiService<T extends HttpServiceType>(serviceType: T) {
  // Sure I know what services there are.
  // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
  return serviceConfig.get(serviceType)! as InstanceType<T>;
}

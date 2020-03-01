import { HttpService } from "./HttpService";
import { AccountService } from "./account/AccountService";
import { AccountServiceMock } from "./account/AccountServiceMock";
import { InstanceService } from "./instance/InstanceService";
import { PersonalAccountService } from "src/apis/identity/PersonalAccountService";
import { PersonalAccountServiceMock } from "src/apis/identity/PersonalAccountServiceMock";
import { DomainsService } from "src/apis/identity/DomainsService";
import { DomainsServiceMock } from "src/apis/identity/DomainsServiceMock";
import { UsersService } from "./identity/UsersService";
import { UsersServiceMock } from "./identity/UsersServiceMock";
import { ProjectsService } from "src/apis/identity/ProjectsService";
import { ProjectsServiceMock } from "src/apis/identity/ProjectsServiceMock";

export const USE_MOCK = false;

const services = [
  [AccountService, USE_MOCK ? AccountServiceMock : AccountService],
  [InstanceService, InstanceService],
  [PersonalAccountService, USE_MOCK ? PersonalAccountServiceMock : PersonalAccountService],
  [DomainsService, USE_MOCK ? DomainsServiceMock : DomainsService],
  [UsersService, USE_MOCK ? UsersServiceMock : UsersService],
  [ProjectsService, USE_MOCK ? ProjectsServiceMock : ProjectsService],
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

import { HttpService } from "./HttpService";
import { AccountService } from "./account/AccountService";
import { AccountServiceMock } from "./account/AccountServiceMock";
import { InstanceService } from "./instance/InstanceService";

export const USE_MOCK = false;

const services = [
  [AccountService, USE_MOCK ? AccountServiceMock : AccountService],
  [InstanceService, InstanceService]
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

import { HttpService, HttpMethod } from '../HttpService';
import { Instance } from "src/models/Instance";

export interface GetInstancesResponse {
  instances: Instance[];
}

export class InstanceService extends HttpService {
  async getInstances(): Promise<GetInstancesResponse> {
    return this.fetch<GetInstancesResponse>({
      method: HttpMethod.GET,
      path: "/resources/instances",
    });
  }
}

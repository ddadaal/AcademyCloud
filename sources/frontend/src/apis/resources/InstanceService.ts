import { HttpService, HttpMethod } from '../HttpService';
import { Instance, Flavor, Image } from "src/models/Instance";

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
  async getFlavors(): Promise<{ flavors: Flavor[] }> {
    return await this.fetch({
      method: HttpMethod.GET,
      path: "/resources/instances/flavors",
    });
  }

  async getImages(): Promise<{ images: Image[] }> {
    return await this.fetch({
      method: HttpMethod.GET,
      path: "/resources/instances/images",
    });
  }

  async createInstance(name: string, flavorId: string, imageId: string, volume: number): Promise<void> {
    return await this.fetch({
      method: HttpMethod.POST,
      path: "/resources/instances",
      body: { name, flavorId, imageId, volume },
    });
  }
}

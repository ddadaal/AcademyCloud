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
      path: "/resources/flavors",
    });
  }

  async getImages(): Promise<{ images: Image[] }> {
    return await this.fetch({
      method: HttpMethod.GET,
      path: "/resources/images",
    });
  }

  async createInstance(name: string, flavorName: string, imageName: string, volume: number): Promise<void> {
    return await this.fetch({
      method: HttpMethod.POST,
      path: "/resources/instances",
      body: { name, flavorName, imageName, volume },
    });
  }

  async stopInstance(instanceId: string) {
    await this.fetch({
      method: HttpMethod.GET,
      path: `/resources/instances/${instanceId}/stop`
    });
  }

  async deleteInstance(instanceId: string) {
    await this.fetch({
      method: HttpMethod.DELETE,
      path: `/resources/instances/${instanceId}`
    });
  }
}

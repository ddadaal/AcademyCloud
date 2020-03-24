import { InstanceService, GetInstancesResponse } from './InstanceService';
import { Flavor,  Image } from "src/models/Instance";

const flavor: Flavor = { name: "m1.nano", cpu: 2, memory: 2, rootDisk: 2 };

export class InstanceServiceMock extends InstanceService {
  async getInstances(): Promise<GetInstancesResponse> {
    await this.delay();

    return {
      instances: [
        { id: "1", name: "test", flavor, status: "SHUTOFF", vmState: "4", taskState: "Building", powerState: "4",
          ip: "10.0.0.162",
          imageName: "cirros", createTime: "2020-03-16T07:27:56.568Z", totalStartupHours: 100 }
      ]
    }
  }

  async getFlavors(): Promise<{ flavors: Flavor[] }> {
    await this.delay();

    return {
      flavors: [flavor]
    }
  }

  async getImages(): Promise<{ images: Image[] }> {
    await this.delay();

    return {
      images: [
        { id: "!2312312", name: "cirros", minDisk: 2 },
        { id: "!2312", name: "ubuntu", minDisk: 40 },
      ]
    }
  }
  async createInstance(name: string, flavorId: string, imageId: string, volume: number): Promise<void> {
    await this.delay();
  }

  async startInstance() {
    await this.delay();
  }

  async stopInstance() {
    await this.delay();
  }

  async rebootInstance() {
    await this.delay();
  }

  async deleteInstance() {
    await this.delay();
  }
}

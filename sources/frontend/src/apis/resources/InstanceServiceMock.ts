import { InstanceService, GetInstancesResponse } from './InstanceService';
import { Flavor, InstanceStatus } from "src/models/Instance";
import { HttpMethod } from '../HttpService';

const flavor: Flavor = { id: "0", name: "m1.nano", cpu: 2, memory: 2, rootDisk: 2 };

export class InstanceServiceMock extends InstanceService {
  async getInstances(): Promise<GetInstancesResponse> {
    await this.delay();

    return {
      instances: [
        { id: "1", name: "test", flavor, status: InstanceStatus.Shutoff, ip: "10.0.0.162", imageName: "cirros", createTime: "2020-03-16T07:27:56.568Z", totalStartupHours: 100 }
      ]
    }
  }

  async getFlavors(): Promise<{ flavors: Flavor[] }> {
    await this.delay();

    return {
      flavors: [flavor]
    }
  }
}

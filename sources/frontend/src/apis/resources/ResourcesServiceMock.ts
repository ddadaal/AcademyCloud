import { ResourcesService, GetResourcesUsedAndLimitsResponse } from "./ResourcesService";

export class ResourcesServiceMock extends ResourcesService {
  async getResourcesUsedAndLimits(): Promise<GetResourcesUsedAndLimitsResponse> {
    await this.delay();

    return {
      allocated: { cpu: 100, memory: 100, storage: 100},
      used: { cpu: 50, memory: 40, storage:60 },
    }
  }
}

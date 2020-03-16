import { HttpService, HttpMethod } from "../HttpService";
import { Resources } from "src/models/Resources";

export interface GetResourcesUsedAndLimitsResponse {
  used: Resources;
  allocated: Resources;
}

export class ResourcesService extends HttpService {

  async getResourcesUsedAndLimits(): Promise<GetResourcesUsedAndLimitsResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: "/resources/limits",
    });

    return resp as GetResourcesUsedAndLimitsResponse;
  }
}

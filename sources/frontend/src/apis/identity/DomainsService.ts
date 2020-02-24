import { HttpService, HttpMethod } from "../HttpService";
import { Domain } from "src/models/Domain";

export interface GetDomainsResponse {
  domains: Domain[];
}

export class DomainsService extends HttpService {
  async getDomains(): Promise<GetDomainsResponse> {
    const resp = await this.fetch<GetDomainsResponse>({
      method: HttpMethod.GET,
      path: "/identity/domains",
    });

    return resp;
  }
}

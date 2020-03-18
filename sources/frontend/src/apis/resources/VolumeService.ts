import { HttpService, HttpMethod } from '../HttpService';
import { Volume } from "src/models/Volume";

export class VolumeService extends HttpService {
  async getVolumes(): Promise<{ volumes: Volume[] }> {
    return await this.fetch({
      method: HttpMethod.GET,
      path: "/resources/volumes",
    });
  }
}

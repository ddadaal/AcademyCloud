import { HttpService, HttpMethod } from "../HttpService";
import { Project } from "src/models/Project";

export interface GetAccessibleProjectsResponse {
  projects: Project[];
}

export class ProjectsService extends HttpService {
  async getAccessibleProjects(): Promise<GetAccessibleProjectsResponse> {
    const resp = await this.fetch({
      method: HttpMethod.GET,
      path: "/identity/projects"
    });

    return resp as GetAccessibleProjectsResponse;
  }
}

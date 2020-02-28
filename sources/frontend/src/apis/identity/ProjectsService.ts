import { HttpService, HttpMethod } from "../HttpService";
import { Project } from "src/models/Project";
import { UserRole } from "src/models/Scope";
import { Resources } from 'src/models/Resources';

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

  async addUserToProject(projectId: string, userId: string, role: UserRole): Promise<void> {
    await this.fetch({
      method: HttpMethod.POST,
      path: `/identity/projects/${projectId}/users/${userId}`,
      body: { role },
    });
  }

  // error: status code 400, { code: "payUser" | "onlyAdmin" }
  async changeUserRole(projectId: string, userId: string, role: UserRole): Promise<void> {
    await this.fetch({
      method: HttpMethod.PATCH,
      path: `/identity/projects/${projectId}/users/${userId}/role`,
      body: { role },
    });
  }

  // error: status code 400, { code: "payUser" | "onlyAdmin" }
  async removeUser(projectId: string, userId: string): Promise<void> {
    await this.fetch({
      method: HttpMethod.DELETE,
      path: `/identity/projects/${projectId}/users/${userId}`,
    });
  }

  async setResources(projectId: string, resources: Resources): Promise<void> {
    await this.fetch({
      method: HttpMethod.PATCH,
      path: `/identity/projects/${projectId}/resources`,
      body: { resources }
    });
  }

  async createProject(name: string, payUserId: string): Promise<void> {
    await this.fetch({
      method: HttpMethod.POST,
      path: `/identity/projects/`,
      body: { name, payUserId }
    });
  }

  // 400, { code:  }
  async deleteProject(projectId: string): Promise<void> {
    await this.fetch({
      method: HttpMethod.DELETE,
      path: `/identity/projects/${projectId}`,
    });
  }

}

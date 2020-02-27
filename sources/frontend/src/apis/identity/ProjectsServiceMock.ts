import { ProjectsService, GetAccessibleProjectsResponse } from "src/apis/identity/ProjectsService";
import { User } from "src/models/User";
import { UserRole } from "src/models/Scope";

const cjd: User = { id: "CJDID", username: "CJD", name: "CJD", active: true };
const cjy: User = { id: "CJYID", username: "CJY", name: "CJY", active: true };
const lq: User = { id: "lqID", username: "lq", name: "lq", active: true };
const fgh: User = { id: "fghID", username: "fgh", name: "fgh", active: true };

export class ProjectsServiceMock extends ProjectsService {
  async getAccessibleProjects(): Promise<GetAccessibleProjectsResponse> {
    await this.delay();
    return {
      projects: [
        { id: "67ID", name: "67", active: true, admins: [lq], payUser: lq, members: [cjd, cjy], resources: { cpu: 4, memory: 8, storage: 512, } },
        { id: "fghID", name: "fgh", active: false, admins: [fgh], payUser: fgh, members: [cjy], resources: { cpu: 4, memory: 8, storage: 512, } },
      ]
    }
  }
  async addUserToProject(projectId: string, userId: string, role: UserRole): Promise<void> {
    await this.delay();
  }

  async changeUserRole(projectId: string, userId: string, role: UserRole): Promise<void> {
    await this.delay();
    throw { code: "payUser" };
  }

  async removeUser(projectId: string, userId: string): Promise<void> {
    await this.delay();
    throw { code: "onlyAdmin" };
  }
}

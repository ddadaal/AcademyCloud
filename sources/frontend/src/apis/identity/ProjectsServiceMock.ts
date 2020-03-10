import { ProjectsService, GetAccessibleProjectsResponse, GetUsersOfProjectResponse } from "src/apis/identity/ProjectsService";
import { User } from "src/models/User";
import { UserRole } from "src/models/Scope";
import { Resources } from "src/models/Resources";

const cjd: User = { id: "CJDID", username: "CJD", name: "CJD",  };
const cjy: User = { id: "CJYID", username: "CJY", name: "CJY" };
const lq: User = { id: "lqID", username: "lq", name: "lq"};
const fgh: User = { id: "fghID", username: "fgh", name: "fgh"};

export class ProjectsServiceMock extends ProjectsService {
  async getAccessibleProjects(): Promise<GetAccessibleProjectsResponse> {
    await this.delay();
    return {
      projects: [
        {
          id: "67ID", name: "67", active: true, admins: [lq], payUser: lq, members: [cjd, cjy], resources: { cpu: 4, memory: 8, storage: 512, },
          userResources: { "lqID": { cpu: 1, memory: 4, storage: 256 }, "CJDID": { cpu: 1, memory: 4, storage: 256 }, "CJYID": { cpu: 2, memory: 2, storage: 128 } }
        },
        {
          id: "fghID", name: "fgh", active: false, admins: [fgh], payUser: fgh, members: [cjy], resources: { cpu: 4, memory: 8, storage: 512, },
          userResources: { "fghID": { cpu: 1, memory: 4, storage: 256 }, "CJYID": { cpu: 2, memory: 2, storage: 128 } }
        },
      ]
    }
  }
  async addUserToProject(projectId: string, userId: string, role: UserRole): Promise<void> {
    await this.delay();
  }

  async setResourcesOfUser(projectId: string, userId: string, resources: Resources): Promise<void> {
    await this.delay();
  }

  async changeUserRole(projectId: string, userId: string, role: UserRole): Promise<void> {
    await this.delay();
    throw { code: "payUser" };
  }

  async removeUserFromProject(projectId: string, userId: string): Promise<void> {
    await this.delay();
    throw { code: "onlyAdmin" };
  }

  async setResources(projectId: string, resources: Resources): Promise<void> {
    await this.delay();
  }

  async createProject(name: string, payUserId: string): Promise<void> {
    await this.delay();
  }

  async deleteProject(projectId: string): Promise<void> {
    await this.delay();
  }

  async getUsersOfProject(projectId: string): Promise<GetUsersOfProjectResponse> {
    await this.delay();
    return {
      admins: [lq],
      members: [cjd, cjy],
      payUser: lq,

      userResources: { "lqID": { cpu: 1, memory: 4, storage: 256 }, "CJDID": { cpu: 1, memory: 4, storage: 256 }, "CJYID": { cpu: 2, memory: 2, storage: 128 } }
    }
  }
  async setPayUser(projectId: string, userId: string): Promise<void> {
    await this.delay();
  }
}


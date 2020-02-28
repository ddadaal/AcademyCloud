import { HttpMethod, HttpService } from "src/apis/HttpService";
import { User } from "src/models/User";

export interface GetAccessibleUsersResponse {
  users: User[];
}

export class UsersService extends HttpService {
  async getAccessibleUsers(): Promise<GetAccessibleUsersResponse> {
    const resp = this.fetch<GetAccessibleUsersResponse>({
      method: HttpMethod.GET,
      path: "/identity/users",
    });

    return resp;
  }

  // 400, { code: "inactive" } | { code: "onlyAdmin" }
  async removeUserFromSystem(userId: string): Promise<void> {
    const resp = this.fetch({
      method: HttpMethod.DELETE,
      path: `/identity/users/${userId}`,
    });
  }


}

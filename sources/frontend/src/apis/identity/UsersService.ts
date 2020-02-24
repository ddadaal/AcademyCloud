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
}

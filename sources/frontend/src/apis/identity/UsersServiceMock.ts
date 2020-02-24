import { UsersService, GetAccessibleUsersResponse } from "src/apis/identity/UsersService";
import { User } from "src/models/User";

const cjd: User = { id: "CJDID", username: "CJD", name: "CJD", active: true };
const cjy: User = { id: "CJYID", username: "CJY", name: "CJY", active: true };

export class UsersServiceMock extends UsersService {
  async getAccessibleUsers(): Promise<GetAccessibleUsersResponse> {
    await this.delay();
    return {
      users: [cjd, cjy]
    };
  }
}

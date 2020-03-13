import { UsersService, GetAccessibleUsersResponse, GetAllUsersResponse } from "src/apis/identity/UsersService";
import { User } from "src/models/User";

const cjd: User = { id: "CJDID",  name: "CJD" };
const cjy: User = { id: "CJYID",  name: "CJY"};

export class UsersServiceMock extends UsersService {
  async getAccessibleUsers(): Promise<GetAccessibleUsersResponse> {
    await this.delay();
    return {
      users: [cjd, cjy]
    };
  }

  async getAllUsers(): Promise<GetAllUsersResponse> {
    await this.delay();
    return {
      users: [
        {...cjd, active: true },
        { ...cjy, active: false},
      ]
    }
  }
}

import { User } from "src/models/User";
import { Resources } from "src/models/Resources";

export interface Project {
  id: string;
  name: string;
  active: boolean;
  admins: User[];
  payUser: User;
  members: User[];
  quota: Resources;

  userQuotas: {[id: string]: Resources};

}

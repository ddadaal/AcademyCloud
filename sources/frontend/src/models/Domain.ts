import { Resources } from "src/models/Resources";
import { User } from "./User";

export interface Domain {
  id: string;
  name: string;
  active: boolean;
  admins: User[];
  payUser: User;
  resources: Resources;
}

import { UserRole } from "src/models/Scope";
import { User } from "src/models/User";
import { Resources } from "src/models/Resources";

export interface FullUser extends User {
  role: UserRole;
  resources?: Resources;
}

export function mergeInformation(admins: User[], members: User[], resources: { [userId: string]: Resources } = {}): FullUser[] {
  return [
    ...admins.map((x) => ({ ...x, role: "admin" as UserRole, resources: resources[x.id] })),
    ...members.map((x) => ({ ...x, role: "member" as UserRole, resources: resources[x.id] })),
  ];

}

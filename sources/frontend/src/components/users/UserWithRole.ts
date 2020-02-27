import { UserRole } from "src/models/Scope";
import { User } from "src/models/User";

export interface UserWithRole extends User {
  role: UserRole;
}

export function mergeAdminAndMember(admins: User[], members: User[]): UserWithRole[] {
  return [
    ...admins.map((x) => ({ ...x, role: "admin" as UserRole })),
    ...members.map((x) => ({ ...x, role: "member" as UserRole })),
  ];
}

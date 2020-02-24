import { UserRole } from "./Scope";

export interface UserDomainAssignment {
  domainId: string;
  domainName: string;
  role: UserRole;
}

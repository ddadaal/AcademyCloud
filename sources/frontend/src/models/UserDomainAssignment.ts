import { UserRole } from './User';

export interface UserDomainAssignment {
  domainId: string;
  domainName: string;
  role: UserRole;
}

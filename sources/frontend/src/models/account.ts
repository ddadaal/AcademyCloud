export type UserRole = "admin" | "member";


export interface Scope {
  domainId: string;
  domainName: string;
  projectId?: string;
  projectName?: string;
  role: UserRole;
}

export interface User {
  domainId: string;
  enabled: boolean;
  id: string;
  name: string;
  description: string;
  email: string;
}

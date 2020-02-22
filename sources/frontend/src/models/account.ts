export type UserRole = "admin" | "member";

export interface User {
  domainId: string;
  enabled: boolean;
  id: string;
  name: string;
  description: string;
  email: string;
}

export type UserRole = "admin" | "member";

export interface User {
  id: string;
  name: string;
  description: string;
  email: string;
}

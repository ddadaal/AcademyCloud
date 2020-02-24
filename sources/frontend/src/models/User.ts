export type UserRole = "admin" | "member";

export interface User {
  id: string;
  username: string;
  name: string;
  description: string;
  email: string;
}

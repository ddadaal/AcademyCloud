export interface ScopeableTarget {
  type: "project" | "domain";
  id: string;
  name: string;
}

export interface Scope extends ScopeableTarget {
  role: "admin" | "member";
}

export interface User {
  domainId: string;
  enabled: boolean;
  id: string;
  name: string;
  description: string;
  email: string;
}

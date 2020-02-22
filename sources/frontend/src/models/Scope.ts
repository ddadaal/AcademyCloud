import { UserRole } from "src/models/User";

export interface Scope {
  system?: boolean;
  domainId: string;
  domainName: string;
  projectId?: string;
  projectName?: string;
  role: UserRole;
}


export function scopeId(scope: Scope): string {
  return `${scope.domainId}+${scope.projectId ?? ""}`;
}

export function scopeName(scope: Scope): string {
  if (scope.projectName) {
    return `${scope.domainName} - ${scope.projectName}`;
  } else {
    return scope.domainName;
  }
}

export function isSystemScope(scope: Scope): boolean {
  return !!scope.system;
}
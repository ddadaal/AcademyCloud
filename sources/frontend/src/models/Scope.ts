export type UserRole = "admin" | "member";


export interface Scope {
  system?: boolean;
  social?: boolean;
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
export function isSocialScope(scope: Scope): boolean {
  return !!scope.social;
}

export function isDomainScope(scope: Scope): boolean {
  return !isSystemScope(scope) && !scope.projectId;
}

export function isAdmin(scope: Scope): boolean {
  return scope.role === "admin";

}
export function isDomainAdmin(scope: Scope): boolean {
  return isDomainScope(scope) && isAdmin(scope);
}

export function isProjectScope(scope: Scope): boolean {
  return !!scope.projectId;
}

export function isProjectAdmin(scope: Scope): boolean {
  return isProjectScope(scope) && isAdmin(scope);
}


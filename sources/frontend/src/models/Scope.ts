import { UserRole } from './account';

export interface Scope {
  system?: boolean;
  domainId: string;
  domainName: string;
  projectId?: string;
  projectName?: string;
  role: UserRole;
}

export class ScopeClass implements Scope {
  system?: boolean | undefined;
  domainId: string;
  domainName: string;
  projectId?: string | undefined;
  projectName?: string | undefined;
  role: UserRole;

  public constructor(scope: Scope) {
    this.system = scope.system;
    this.domainId = scope.domainId;
    this.domainName = scope.domainName;
    this.projectId = scope.projectId;
    this.projectName = scope.projectName;
    this.role = scope.role;
  }

  get scopeId(): string {
    return `${this.domainId}+${this.projectId ?? ""}`;
  }

  get scopeName(): string {
    if (this.projectName) {
      return `${this.domainName} - ${this.projectName}`;
    } else {
      return this.domainName;
    }
  }

}

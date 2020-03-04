export enum TransactionType {
  Charge = "Charge",
  UserManagementFee = "UserManagementFee",
  DomainManagementFee = "DomainManagementFee",
  ProjectManagementFee = "ProjectManagementFee",
  DomainResources = "DomainResources",
  ProjectResources = "ProjectResources",
}

export interface TransactionReason {
  type: TransactionType;
  info?: string;
}

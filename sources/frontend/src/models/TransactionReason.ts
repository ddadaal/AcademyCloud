export enum TransactionType {
  Charge = "Charge",
  UserManagementFee = "UserManagementFee",
  DomainManagementFee = "DomainManagementFee",
  ProjectManagementFee = "ProjectManagementFee",
  DomainResources = "DomainResources",
  ProjectResources = "ProjectResources",
  DomainQuotaChange = "DomainQuotaChange",
  ProjectQuotaChange = "ProjectQuotaChange",
}

export interface TransactionReason {
  type: TransactionType;
  info?: string;
}

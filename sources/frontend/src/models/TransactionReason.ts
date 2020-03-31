export enum TransactionType {
  Charge = "Charge",
  UserManagementFee = "UserManagementFee",
  DomainManagementFee = "DomainManagementFee",
  ProjectManagementFee = "ProjectManagementFee",
  DomainQuota = "DomainQuota",
  ProjectQuota = "ProjectQuota",
  DomainQuotaChange = "DomainQuotaChange",
  ProjectQuotaChange = "ProjectQuotaChange",
  SocialResourcesChange = "SocialResourcesChange",
  UserProjectQuota = "UserProjectQuota",
}

export interface TransactionReason {
  type: TransactionType;
  info?: string;
}

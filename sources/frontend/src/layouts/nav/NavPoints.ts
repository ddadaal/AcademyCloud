import lang from "src/i18n/lang";

export interface NavPoint {
  path: string | RegExp; // 匹配的path，是string的时候用startsWith判断，是RegExp的时候用match判断
  jumpTo?: (pathname: string) => string; // 传给Link的path，支持..返回上一级
  textId: string; //
}

const root = lang.nav.sidenav;

function getBillingSubject(pathname: string) {
  const splitted = pathname.split("/").filter((x) => !!x);
  return splitted[4];
}

export const availableNavPoints = [
  { path: "/resources", textId: root.resources.root },
  { path: "/resources/dashboard", textId: root.resources.dashboard },
  { path: "/resources/instances", textId: root.resources.instance },
  { path: "/resources/network", textId: root.resources.network },
  { path: "/resources/volumes", textId: root.resources.volume },
  { path: "/expenses", textId: root.expenses.root },
  { path: "/expenses/overview", textId: root.expenses.overview },
  { path: "/expenses/transactions/account", textId: root.expenses.accountTransactions },
  { path: "/expenses/transactions/system", textId: root.expenses.systemTransactions },
  { path: "/expenses/transactions/domain", textId: root.expenses.domainTransactions },
  { path: "/expenses/transactions/project", textId: root.expenses.projectTransactions },
  { path: "/expenses/billings/domains", textId: root.expenses.billings.domains },
  { path: "/expenses/billings/domains/allocated", textId: root.expenses.billings.allocated },
  {
    path: /\/expenses\/billings\/domains\/allocated\/[0-9a-zA-Z]*/,
    textId: root.expenses.billings.detail,
    jumpTo: (pathname: string) => `/expenses/billings/domains/allocated/${getBillingSubject(pathname)}`,
  },
  { path: "/expenses/billings/domains/used", textId: root.expenses.billings.allocated },
  { path: "/expenses/billings/domain", textId: root.expenses.billings.domain },
  { path: "/expenses/billings/domain/allocated", textId: root.expenses.billings.allocated },
  { path: "/expenses/billings/domain/used", textId: root.expenses.billings.used },
  { path: "/expenses/billings/projects", textId: root.expenses.billings.projects },
  { path: "/expenses/billings/projects/allocated", textId: root.expenses.billings.allocated },
  { path: "/expenses/billings/projects/used", textId: root.expenses.billings.used },
  { path: "/expenses/billings/project", textId: root.expenses.billings.project },
  { path: "/expenses/billings/project/allocated", textId: root.expenses.billings.allocated },
  { path: "/expenses/billings/project/used", textId: root.expenses.billings.used },
  { path: "/expenses/billings/users", textId: root.expenses.billings.users },
  { path: "/expenses/billings/users/allocated", textId: root.expenses.billings.allocated },
  { path: "/expenses/billings/users/used", textId: root.expenses.billings.used },
  { path: "/expenses/billings/user", textId: root.expenses.billings.user },
  { path: "/expenses/billings/user/allocated", textId: root.expenses.billings.allocated },
  { path: "/expenses/billings/user/used", textId: root.expenses.billings.used },
  { path: "/identity", textId: root.identity.root },
  { path: "/identity/account", textId: root.identity.account.root },
  { path: "/identity/account/basic", textId: root.identity.account.basic },
  { path: "/identity/account/joinedDomains", textId: root.identity.account.domains },
  { path: "/identity/projects", textId: root.identity.projects },
  { path: "/identity/domains", textId: root.identity.domains },
  { path: "/identity/users", textId: root.identity.users },
] as NavPoint[];

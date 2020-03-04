import lang from "src/i18n/lang";

export interface NavPoint {
  path: string | RegExp; // 匹配的path，是string的时候用startsWith判断，是RegExp的时候用match判断
  jumpTo?: (pathname: string) => string; // 传给Link的path，支持..返回上一级
  textId: string; //
}

const root= lang.nav.sidenav;

function getInvreqId(pathname: string) {
  const splitted = pathname.split("/").filter((x) => !!x);
  return splitted[1];
}

export const availableNavPoints = [
  { path: "/resources", textId: root.resources.root },
  { path: "/resources/dashboard", textId: root.resources.dashboard },
  { path: "/resources/instances", textId: root.resources.instance },
  { path: "/resources/network", textId: root.resources.network },
  { path: "/resources/volumes", textId: root.resources.volume },
  { path: "/expenses", textId: root.expenses.root },
  { path: "/expenses/overview", textId: root.expenses.overview },
  { path: "/expenses/accountTransactions", textId: root.expenses.accountTransactions },
  { path: "/expenses/systemTransactions", textId: root.expenses.accountTransactions },
  { path: "/expenses/domainTransactions", textId: root.expenses.accountTransactions },
  { path: "/expenses/projectTransactions", textId: root.expenses.accountTransactions },
  { path: "/identity", textId: root.identity.root },
  { path: "/identity/account", textId: root.identity.account.root },
  { path: "/identity/account/basic", textId: root.identity.account.basic },
  { path: "/identity/account/joinedDomains", textId: root.identity.account.domains },
  { path: "/identity/projects", textId: root.identity.projects },
  { path: "/identity/domains", textId: root.identity.domains },
  { path: "/identity/users", textId: root.identity.users },
] as NavPoint[];

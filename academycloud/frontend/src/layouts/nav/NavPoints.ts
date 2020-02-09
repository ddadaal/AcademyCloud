import lang from "src/i18n/lang";

export interface NavPoint {
  path: string | RegExp; // 匹配的path，是string的时候用startsWith判断，是RegExp的时候用match判断
  jumpTo?: (pathname: string) => string; // 传给Link的path，支持..返回上一级
  textId: string; //
}

const root = lang.nav.navPoints;

function getInvreqId(pathname: string) {
  const splitted = pathname.split("/").filter((x) => !!x);
  return splitted[1];
}

export const availableNavPoints = [
  { path: "/resources", textId: root.resources.root },
  { path: "/resources/instances", textId: root.resources.instance},
  { path: "/resources/network", textId: root.resources.network},
  { path: "/resources/volumes", textId: root.resources.volume},
] as NavPoint[];

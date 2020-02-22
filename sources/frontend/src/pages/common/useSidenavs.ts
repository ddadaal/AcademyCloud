import { NavItemProps } from "src/layouts/nav/NavItemProps";
import { useStore } from "simstate";
import { NavStore } from "src/layouts/nav/NavStore";
import { useMemo, useEffect } from "react";

export function useSidenavs(sidebarRoutes: NavItemProps[], basePath: string) {
  const navStore = useStore(NavStore);

  const routes = useMemo(() => (
    sidebarRoutes.map((x) => ({ ...x, path: basePath + x.path }))
  ), [sidebarRoutes, basePath]);

  useEffect(() => {
    navStore.setSidenavs(routes);
    return () => { navStore.setSidenavs([]); }
  }, [routes]);
}

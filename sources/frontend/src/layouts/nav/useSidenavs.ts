import { useEffect, useMemo } from 'react';
import { useStore } from 'simstate';
import { NavStore } from './NavStore';
import { NavItemProps } from "src/layouts/nav/NavItemProps";

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

import { useEffect } from 'react';
import { useStore } from 'simstate';
import { NavStore } from './NavStore';
import { NavItemProps } from "src/layouts/nav/NavItemProps";

export function useSidenavs(sidebarRoutes: NavItemProps[]) {
  const navStore = useStore(NavStore);

  useEffect(() => {
    navStore.setSidenavs(sidebarRoutes);

    return () => { navStore.setSidenavs([]); }
  }, [navStore]);
}

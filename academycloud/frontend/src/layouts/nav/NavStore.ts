import { availableNavPoints, NavPoint } from "./NavPoints";
import { useState, useMemo } from "react";
import { useLocation } from "src/utils/useLocation";
import { NavItemProps } from "./NavItemProps";

interface SideNav {
  navs: NavItemProps[];
}

export function NavStore() {
  const location = useLocation();

  const [sidenavs, setSidenavs] = useState<NavItemProps[]>([]);

  const hasSider = useMemo(() => sidenavs.length !== 0, [sidenavs]);

  const [sidebarCollapsed, setSidebarCollapsed] = useState(false);

  const currentNavPath = useMemo(() => {
    const path = location.location.pathname;
    const points = availableNavPoints.map((x) => {
      if (typeof x.path === "string") {
        return path.startsWith(x.path) ? x : null;
      } else {
        const match = path.match(x.path);
        if (match) {
          return { ...x, path: x.jumpTo?.(path) ?? x.path };
        } else {
          return null;
        }
      }
    }).filter((x) => !!x) as NavPoint[];
    return points;
  } ,[location]);

  return { currentNavPath, sidebarCollapsed, setSidebarCollapsed, sidenavs, setSidenavs, location, hasSider};

}



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
    const path = location.pathname;
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

    // remove the shorter one if two have the same depth
    const filteredOut = [] as NavPoint[];
    const depth = (path: string) => path.split("/").length;
    for (let i = 0; i < points.length; i++) {
      for (let j = i + 1; j < points.length; j++) {
        const path1 = points[i].path as string;
        const path2 = points[j].path as string;
        if (depth(path1) === depth(path2)) {
          if (path1.length < path2.length) {
            filteredOut.push(points[i]);
          } else {
            filteredOut.push(points[j]);
          }
        }
      }
    }

    return points.filter(x => !filteredOut.includes(x));
  }, [location]);

  return { currentNavPath, sidebarCollapsed, setSidebarCollapsed, sidenavs, setSidenavs, location, hasSider };

}



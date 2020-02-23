import { NavItemProps } from '../NavItemProps';

export function startsWithMatch(item: NavItemProps, path: string): boolean {
  return item.match?.(path) ?? path.startsWith(item.path);
}

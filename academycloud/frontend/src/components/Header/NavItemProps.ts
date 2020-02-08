export interface NavItemProps {
  path: string;
  text: string;
  iconName: string;
  match(path: string): boolean;
  children?: NavItemProps[];
}

import React from "react";

export interface NavItemProps {
  path: string;
  textId: string;
  Icon: React.ForwardRefExoticComponent<{}> ;
  match(path: string): boolean;
  children?: NavItemProps[];
}

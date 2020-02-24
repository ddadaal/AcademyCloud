import React from "react";
import { Breadcrumb } from "antd";
import { NavStore } from "./NavStore";
import { useStore } from "simstate";
import { Link } from "@reach/router";
import { Localized, lang } from "src/i18n";

interface Props {
  className?: string;
}

export const BreadcrumbNav: React.FC<Props> = ({ className }) => {
  const navStore = useStore(NavStore);
  return (
    <Breadcrumb className={className}>
      {navStore.currentNavPath.map((x) =>
        <Breadcrumb.Item key={x.path as string}>
          <Link to={x.path as string}><Localized id={x.textId}/></Link>
        </Breadcrumb.Item>,
      )}
    </Breadcrumb>
  );
}

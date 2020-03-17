import React from "react";
import { Section } from "src/pages/Expenses/Overview/Section";
import { lang, Localized } from "src/i18n";
import { Link } from "@reach/router";
import { InstanceTable } from "src/components/instance/InstanceTable";

const root = lang.resources;



export const InstanceList: React.FC = () => {
  return (
    <Section
      title={<Localized id={root.dashboard.instanceList.title} />}
      extra={<Link to="../instances"><Localized id={root.dashboard.instanceList.detail} /></Link>}>
      <InstanceTable />
    </Section>
  )
};

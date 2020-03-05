import React from "react";
import { Stats } from "src/components/billings/StatsPage/Stats";
import { BillType, BillSubjectType } from "src/models/Billings";
import { Localized, lang } from "src/i18n";
import { Section } from "src/pages/Expenses/Overview/Section";
import { Link } from "@reach/router";

interface Props {
  domainId: string;
  domainName: string;
}

const root = lang.components.billings;
const root2 = lang.expenses.overview.domainAdmin;

export const DomainAdmin: React.FC<Props> = ({ domainId, domainName }) => {

  return (
    <Section title={(
      <Localized id={root.stats.title} replacements={[
        <Localized key="subjectType" id={root.subjectType.domain} />,
        domainName,
        <Localized key="billType" id={root.allocated} />,
      ]} />
    )} extra={<Link to="../billings/domain/allocated"><Localized id={root2.link} /></Link>}>
      <Stats
        billType={BillType.Allocated}
        billSubjectType={BillSubjectType.domain}
        id={domainId}
      />
    </Section>
  )
}

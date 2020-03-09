import React from "react";
import { Stats } from "src/components/billings/StatsPage/Stats";
import { BillType, BillSubjectType } from "src/models/Billings";
import { useRefreshToken } from "src/utils/refreshToken";
import { Localized, lang } from "src/i18n";
import { Section } from "src/pages/Expenses/Overview/Section";
import { Link } from "@reach/router";

interface Props {
  projectId: string;
  projectName: string;
}

const root = lang.components.billings;
const root2 = lang.expenses.overview.projectAdmin;

export const ProjectAdmin: React.FC<Props> = ({ projectId, projectName }) => {

  return (
    <Section title={(
      <Localized id={root.stats.title} replacements={[
        <Localized key="subjectType" id={root.subjectType.Project} />,
        projectName,
        <Localized key="billType" id={root.Allocated} />,
      ]} />
    )} extra={<Link to="../billings/project/allocated"><Localized id={root2.link} /></Link>}>
      <Stats
        billType={BillType.Allocated}
        billSubjectType={BillSubjectType.Project}
        id={projectId}
      />
    </Section>
  )
}

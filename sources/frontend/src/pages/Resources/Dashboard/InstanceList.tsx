import React from "react";
import { Section } from "src/pages/Expenses/Overview/Section";
import { lang, Localized } from "src/i18n";
import { Link } from "@reach/router";
import { InstanceTable } from "src/components/instance/InstanceTable";
import { getApiService } from "src/apis";
import { InstanceService } from "src/apis/resources/InstanceService";
import { useAsync } from "react-async";

const root = lang.resources;


const service = getApiService(InstanceService);

const getInstances = () => service.getInstances().then(x => x.instances);

export const InstanceList: React.FC = () => {
  const { data, isPending } = useAsync({ promiseFn: getInstances });
  return (
    <Section
      title={<Localized id={root.dashboard.instanceList.title} />}
      extra={<Link to="../instances"><Localized id={root.dashboard.instanceList.detail} /></Link>}>
      <InstanceTable data={data} loading={isPending}/>
    </Section>
  )
};

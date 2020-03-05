import React from "react";
import { Resources, resourcesString } from "src/models/Resources";
import { ModalLink } from "src/components/ModalLink";
import { ResourcesViewTable } from "src/components/resources/ResourcesViewTable";
import { Localized, lang } from "src/i18n";

interface Props {
  resources: Resources;
}

const root = lang.components.resources;

export const ResourcesModalLink: React.FC<Props> = ({ resources }) => {
  return (
    <ModalLink modalTitle={<Localized id={root.modalTitle} />} modalContent={<ResourcesViewTable resources={resources} />}>
      {resourcesString(resources)}
    </ModalLink>
  )

}

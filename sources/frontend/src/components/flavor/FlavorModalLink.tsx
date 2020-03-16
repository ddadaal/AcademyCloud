import React from "react";
import { Resources, resourcesString } from "src/models/Resources";
import { ModalLink } from "src/components/ModalLink";
import { ResourcesViewTable } from "src/components/resources/ResourcesViewTable";
import { Localized, lang } from "src/i18n";
import { Flavor, flavorString } from 'src/models/Instance';
import { FlavorViewTable } from "src/components/flavor/FlavorViewTable";

interface Props {
  flavor: Flavor;
}

const root = lang.components.flavor;

export const FlavorModalLink: React.FC<Props> = ({ flavor }) => {
  return (
    <ModalLink modalTitle={<Localized id={root.title} />} modalContent={<FlavorViewTable flavor={flavor} />}>
      {flavor.name}
    </ModalLink>
  )

}

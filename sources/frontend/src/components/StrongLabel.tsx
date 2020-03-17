import React from "react";
import { Localized } from 'src/i18n';

export const StrongLabel: React.FC<{ id: string }> = ({ id }) => {
  return (
    <strong>
      <Localized id={id} />
    </strong>
  )
};


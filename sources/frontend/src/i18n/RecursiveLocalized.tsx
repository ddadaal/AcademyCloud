import React from "react";
import { Localized } from ".";

export type RecursiveLocalizedId = string | [string, RecursiveLocalizedId[]];

interface Props {
  id: RecursiveLocalizedId;
}

export const RecursiveLocalized: React.FC<Props> = ({ id }) => {
  if (typeof id === "string") {
    return <Localized id={id} />;
  } else {
    return (
      <Localized
        id={id[0]}
        replacements={id[1].map((x, i) => <RecursiveLocalized key={i} id={x} />)}
      />
    );
  }
}

import React from "react";
import { Helmet, HelmetProps } from "react-helmet";
import { useStore } from "simstate";
import { I18nStore } from "src/i18n";

interface Props extends HelmetProps {
  title?: string;
  titleId?: string;
}

export const PageMetadata: React.FC<Props> = ({ title, titleId, ...rest }) => {

  const i18nStore = useStore(I18nStore);

  const actualTitle = title ? title : titleId ? i18nStore.translate(titleId) : null;

  const titleStr = `${actualTitle ? `${actualTitle} | ` : ""}AcademyCloud`;

  return (
    <Helmet
      title={titleStr}
      {...rest}
    />
  )
}

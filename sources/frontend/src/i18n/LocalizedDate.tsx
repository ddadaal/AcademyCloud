import React, { useMemo } from "react";
import { useStore } from 'simstate';
import { I18nStore } from '.';
import dayjs from "dayjs";

import "dayjs/locale/en";
import "dayjs/locale/zh-cn";

dayjs.locale("en");

interface Props {
  dateTimeString: string;
}

export const LocalizedDate: React.FC<Props> = ({ dateTimeString }) => {
  const i18nStore = useStore(I18nStore);

  const locale = useMemo(() => i18nStore.currentLanguage.metadata.dayjsLocale, [i18nStore.currentLanguage]);

  return (
    <>
      {dayjs(dateTimeString).locale(locale).format()}
    </>
  )
}

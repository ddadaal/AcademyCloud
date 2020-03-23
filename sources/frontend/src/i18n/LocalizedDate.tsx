import React, { useMemo } from "react";
import { useStore } from 'simstate';
import { I18nStore } from '.';
import dayjs from "dayjs";

interface Props {
  dateTimeString: string;
}

export const LocalizedDate: React.FC<Props> = ({ dateTimeString }) => {

  return (
    <>
      {dayjs(dateTimeString).format("YYYY-MM-DD HH:mm:ss")}
    </>
  )
}

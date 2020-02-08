import React from "react";
import { useStore } from "simstate";
import I18nStore from "./I18nStore";

export function useLocalized(id: string, replacements?: React.ReactNode[]) {
  const i18nStore = useStore(I18nStore);

  return i18nStore.translate(id, replacements);
}

export function useMultiLocalized(...values: (string | [string, React.ReactNode[] | undefined])[]): React.ReactNode[] {
  const i18nStore = useStore(I18nStore);

  return values.map((value) =>
    typeof value === "string"
      ? i18nStore.translate(value)
      : i18nStore.translate(value[0], value[1]));

}


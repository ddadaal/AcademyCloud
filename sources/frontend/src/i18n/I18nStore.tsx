import { getLanguage } from "src/i18n/definition";
import { useState, useCallback } from "react";


function getInitialLanguage(): string {
  return navigator.language;
}

const splitter = /(\{\})/;

function replacePlaceholders(definition: string, replacements: React.ReactNode[]): React.ReactNode | string {
  const array = definition.split(splitter) as React.ReactNode[];
  let ri = 0;

  let containsNonPrimitive = false;

  for (let i = 1; i < array.length; i += 2) {
    if (typeof replacements[ri] === "object") {
      containsNonPrimitive = true;
    }
    array[i] = replacements[ri++];
  }

  if (!containsNonPrimitive) {
    return array.join("");
  }

  return array;
}

export default function I18nStore() {
  const [currentLanguage, setLanguage] = useState(getLanguage(getInitialLanguage()));

  const changeLanguage = useCallback((langString: string) => setLanguage(getLanguage(langString)), []);

  const getDefinition = useCallback((id: string): string => {
    let content = currentLanguage.definitions;
    for (const key of id.split(".")) {
      if (typeof content === "undefined") {
        throw new RangeError(`unidentified id ${id}`);
      }
      content = content[key];
    }
    if (typeof content !== "string") {
      throw new RangeError(`id ${id} does not refer to a string. actual value: ${content}`);
    }
    return content;
  }, [currentLanguage]);

  const translate = useCallback((id: string, replacements?: React.ReactNode[]): React.ReactNode | string => {

    const def = getDefinition(id);
    if (!replacements || replacements.length === 0) {
      return def;
    }
    return replacePlaceholders(def, replacements);
  }, [getDefinition]);

  return { currentLanguage, changeLanguage, setLanguage, translate };

}


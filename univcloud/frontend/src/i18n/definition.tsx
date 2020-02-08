import cn from "./definitions/cn";
import en from "./definitions/en";

export const allLanguages = [cn, en];

export const baselineLanguage = cn;

export type Language = typeof baselineLanguage;

export function getLanguage(langString: string): Language {
  const language = allLanguages.find((x) => x.metadata.langStrings.includes(langString));
  if (!language) { throw `Invalid lang string ${langString}`; }
  return language;
}

export function getDefinitions(langString: string): Language["definitions"] {
  return getLanguage(langString).definitions;
}

export type Definitions = ReturnType<typeof getDefinitions>;

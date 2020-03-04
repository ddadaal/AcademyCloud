import React from "react";
import { lang, Localized } from "src/i18n"

const root = lang.validateMessages;

export const required = <Localized id={root.required} />;
export const number = <Localized id={root.number} />;
export const integer = <Localized id={root.integer} />;
export const email = <Localized id={root.email}/>


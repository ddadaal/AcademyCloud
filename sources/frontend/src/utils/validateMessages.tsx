import React from "react";
import { lang, Localized } from "src/i18n"

const root = lang.validateMessages;

export const required = <Localized id={root.required} />;
export const email = <Localized id={root.email}/>


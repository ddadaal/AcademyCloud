import React from "react";
import { TransactionType } from 'src/models/TransactionReason';
import { lang, Localized } from "src/i18n";

const root = lang.components.transactions.type;

interface Props {
  type: TransactionType;
}

export const TransactionTypeText: React.FC<Props> = ({ type }) => {
  return <Localized id={root[type]} />
}

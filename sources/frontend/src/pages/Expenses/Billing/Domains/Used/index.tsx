import React from "react";
import { DomainsUsed } from "src/pages/Expenses/Billing/Domains/Used/DomainsUsed";
import { Router } from "@reach/router";
import { HistoryDomainUsed } from "src/pages/Expenses/Billing/Domains/Used/HistoryDomainUsed";

export default function () {
  return (
    <Router>
      <DomainsUsed path="/" />
      <HistoryDomainUsed path=":domainId" />
    </Router>
  )
}

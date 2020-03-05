import React from "react";
import { DomainsAllocated } from "src/pages/Expenses/Billing/Domains/Allocated/DomainsAllocated";
import { Router } from "@reach/router";
import { HistoryDomainAllocated } from "src/pages/Expenses/Billing/Domains/Allocated/HistoryDomainAllocated";

export default function () {
  return (
    <Router>
      <DomainsAllocated path="/" />
      <HistoryDomainAllocated path=":domainId" />
    </Router>
  )
}

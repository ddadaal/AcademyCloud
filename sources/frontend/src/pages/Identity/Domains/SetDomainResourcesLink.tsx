import React, { useCallback } from "react";
import { Domain } from "src/models/Domain";
import { getApiService } from "src/apis";
import { DomainsService } from "src/apis/identity/DomainsService";
import { SetResourcesLink } from "src/components/resources/SetResourcesLink";
import { Resources } from "src/models/Resources";
import { QuotaService } from "src/apis/expenses/QuotaService";
import { BillSubjectType } from "src/models/Billings";

interface Props {
  domain: Domain;
  reload: () => void;
}

const service = getApiService(DomainsService);

const quotaService = getApiService(QuotaService);

export const SetDomainResourcesLink: React.FC<Props> = ({ domain, reload }) => {


  const onConfirm = useCallback(async (resources: Resources) => {
    await service.setResources(domain.id, resources);
    reload();
  }, [domain.id, reload]);

  const getAvailableQuota = useCallback(async () => {
    return await quotaService.getQuotaStatus(BillSubjectType.System, "");
  }, []);

  return (
    <SetResourcesLink getAvailableQuota={getAvailableQuota} initial={domain.quota} onConfirm={onConfirm} />
  );
}

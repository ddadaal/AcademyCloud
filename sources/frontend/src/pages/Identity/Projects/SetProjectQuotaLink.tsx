import React, { useCallback } from "react";
import { Domain } from "src/models/Domain";
import { getApiService } from "src/apis";
import { SetResourcesLink } from "src/components/resources/SetResourcesLink";
import { Resources } from "src/models/Resources";
import { ProjectsService } from "src/apis/identity/ProjectsService";
import { Project } from "src/models/Project";
import { QuotaService } from "src/apis/expenses/QuotaService";
import { BillSubjectType } from "src/models/Billings";

interface Props {
  domainId: string;
  project: Project;
  reload: () => void;
}

const service = getApiService(ProjectsService);

const quotaService = getApiService(QuotaService);

export const SetProjectQuotasLink: React.FC<Props> = ({ project, reload, domainId }) => {

  const onConfirm = useCallback(async (resources: Resources) => {
    await service.setResources(project.id, resources);
    reload();
  }, [project.id, reload]);

  const getAvailableQuota = useCallback(async () => {
    return await quotaService.getQuotaStatus(BillSubjectType.Domain, domainId);
  }, [domainId]);

  return (
    <SetResourcesLink getAvailableQuota={getAvailableQuota} initial={project.quota} onConfirm={onConfirm} />
  );
}

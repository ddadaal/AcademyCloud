import React, { useCallback } from "react";
import { Domain } from "src/models/Domain";
import { getApiService } from "src/apis";
import { SetResourcesLink } from "src/components/resources/SetResourcesLink";
import { Resources } from "src/models/Resources";
import { ProjectsService } from "src/apis/identity/ProjectsService";
import { Project } from "src/models/Project";

interface Props {
  project: Project;
  reload: () => void;
}

const service = getApiService(ProjectsService);

export const SetProjectResourcesLink: React.FC<Props> = ({ project, reload }) => {

  const onConfirm = useCallback(async (resources: Resources) => {
    await service.setResources(project.id, resources);
    reload();
  }, [project.id, reload]);

  return (
    <SetResourcesLink initial={project.resources} onConfirm={onConfirm} />
  );
}

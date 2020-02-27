import React, { useState } from "react";
import { Localized, lang } from "src/i18n";
import { getApiService } from "src/apis";
import { DomainsService } from "src/apis/identity/DomainsService";
import { useAsync } from "react-async";
import Modal from "antd/lib/modal/Modal";
import { ResourcesEditForm } from "src/components/resources/ResourcesEditForm";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { Project } from "src/models/Project";
import { ProjectsService } from "src/apis/identity/ProjectsService";

interface Props {
  reload: () => void;
  project: Project;
}
const root = lang.identity.projects.table;
const opResult = lang.components.operationResult;

const service = getApiService(ProjectsService);

const updateResources = async ([domainId, resources]) => {
  await service.setResources(domainId, resources);
}

export const SetResourcesLink: React.FC<Props> = ({ project, reload }) => {

  const [modalShown, setModalShown] = useState(false);

  const [resources, setResources] = useState(project.resources);

  const [api, contextHolder] = useLocalizedNotification();

  const { run, isPending } = useAsync({
    deferFn: updateResources,
    onResolve: () => {
      reload();
      setModalShown(false);
      api.success({ messageId: [opResult.success, [root.setResources.title]] });
    },
    onReject: () => {
      api.error({ messageId: [opResult.fail, [root.setResources.title]] });
    }
  });

  return (
    <>
      <a onClick={() => setModalShown(true)}>
        <Localized id={root.setResources.title} />
      </a>
      {contextHolder}
      <Modal
        visible={modalShown}
        title={<Localized id={root.setResources.title} />}
        onOk={() => run(project.id, resources)}
        onCancel={() => setModalShown(false)}
        confirmLoading={isPending}
      >
        <ResourcesEditForm initial={resources} onValuesChange={setResources} />
      </Modal>
    </>
  );
}

import React, { useCallback } from "react";
import { lang, Localized } from "src/i18n";
import { Project } from "src/models/Project";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { Modal, Popover, Tooltip } from "antd";
import { ExclamationOutlined } from "@ant-design/icons";
import { getApiService } from "src/apis";
import { ProjectsService } from "src/apis/identity/ProjectsService";

interface Props {
  project: Project;
  reload: () => void;
}

const root = lang.identity.projects.table.delete;
const opResult = lang.components.operationResult;

const service = getApiService(ProjectsService);
const deleteProject = ([projectId]: [string]) => {
  return service.deleteProject(projectId);
}

export const DeleteProjectLink: React.FC<Props> = ({ project, reload }) => {
  const [api, contextHolder] = useLocalizedNotification();

  const [modalApi, modalContextHolder] = Modal.useModal();

  const onClick = useCallback(() => {
    modalApi.confirm({
      title: <Localized id={root.confirmPrompt} replacements={[project.name]} />,
      icon: <ExclamationOutlined />,
      onOk: () => deleteProject([project.name])
        .then(() => {
          reload();
          api.success({
            messageId: [opResult.success, [root.opName]],
          })
        })
        .catch(({ code }) => api.error({
          messageId: [opResult.fail, [root.opName]],
        })),
    });
  }, [project, reload]);

  return (
    <span>
      {contextHolder}
      {modalContextHolder}
      {project.active
        ? (
          <a onClick={onClick}>
            <Localized id={root.link} />
          </a>
        )
        : (
          <Tooltip title={<Localized id={root.inactive} />}>
            <span>
              <Localized id={root.link} />
            </span>
          </Tooltip>
        )
      }
    </span>
  );
}

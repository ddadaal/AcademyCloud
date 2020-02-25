import React, { useState } from "react";
import { Domain } from "src/models/Domain";
import { Localized, lang } from "src/i18n";
import { getApiService } from "src/apis";
import { DomainsService } from "src/apis/identity/DomainsService";
import { useAsync } from "react-async";
import Modal from "antd/lib/modal/Modal";
import { useLocalizedNotification } from "src/utils/useLocalizedNotification";
import { UsersSelectionMenu } from "src/components/users/UsersSelectionMenu";
import { UsersService } from "src/apis/identity/UsersService";
import { User } from "src/models/User";

interface Props {
  domain: Domain;
  reload: () => void;
}
const root = lang.identity.domains.setAdmins;
const opResult = lang.components.operationResult;

const domainsService = getApiService(DomainsService);

const updateAdmins = async ([domainId, admins]: [string, User[]]) => {
  await domainsService.setAdmins(domainId, admins.map((x) =>x.id));
}

const usersService = getApiService(UsersService);

const getUsers = () => usersService.getAccessibleUsers().then((resp) => resp.users);

export const SetAdminLink: React.FC<Props> = ({ domain, reload }) => {

  const [modalShown, setModalShown] = useState(false);

  const [admins, setAdmins] = useState(domain.admins);

  const [api, contextHolder] = useLocalizedNotification();

  const { run, isPending } = useAsync({
    deferFn: updateAdmins,
    onResolve: () => {
      reload();
      setModalShown(false);
      api.success({ messageId: [opResult.success, [root.title]]});
    },
    onReject: () => {
      api.error({ messageId: [opResult.success, [root.title]]});
    }
  });

  return (
    <>
      <a onClick={() => setModalShown(true)}>
        <Localized id={root.title} />
      </a>
      {contextHolder}
      <Modal
        visible={modalShown}
        title={<Localized id={root.title} />}
        onOk={() => run(domain.id, admins)}
        onCancel={() => setModalShown(false)}
        confirmLoading={isPending}
        destroyOnClose={true}
      >
        <UsersSelectionMenu getAllUsers={getUsers} selected={admins} onChange={setAdmins}  />
      </Modal>
    </>
  );
}

import React, { useCallback } from "react";
import { FullUser } from '../FullUser';
import { Resources } from 'src/models/Resources';
import { Divider } from "antd";
import { SetResourcesLink } from "src/components/resources/SetResourcesLink";

interface Props {
  user: FullUser;
  onConfirm?: (user: FullUser, resources: Resources) => Promise<void>;
  getAvailableQuota: (userId: string) => Promise<{ used: Resources; total: Resources }>;
}

export const SetUserResourcesLink: React.FC<Props> = ({ user, onConfirm, getAvailableQuota}) => {

  const handleConfirm = useCallback(async (resources: Resources) => {
    await onConfirm?.(user, resources);
  }, [onConfirm, user]);

  const handleAvailableQuota = useCallback(async () => {
    return await getAvailableQuota(user.id);
  }, [user]);

  if (user.resources) {
    return (
      <>
        <Divider type="vertical" />
        <SetResourcesLink getAvailableQuota={handleAvailableQuota} initial={user.resources} onConfirm={handleConfirm} />
      </>
    )
  } else {
    return null;
  }
}

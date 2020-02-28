import React, { useCallback } from "react";
import { FullUser } from '../FullUser';
import { Resources } from 'src/models/Resources';
import { Divider } from "antd";
import { SetResourcesLink } from "src/components/resources/SetResourcesLink";

interface Props {
  user: FullUser;
  onConfirm?: (user: FullUser, resources: Resources) => Promise<void>;
}

export const SetUserResourcesLink: React.FC<Props> = ({ user, onConfirm }) => {

  const handleConfirm = useCallback(async (resources: Resources) => {
    await onConfirm?.(user, resources);
  }, [onConfirm, user]);

  if (user.resources) {
    return (
      <>
        <Divider type="vertical" />
        <SetResourcesLink initial={user.resources} onConfirm={handleConfirm} />
      </>
    )
  } else {
    return null;
  }
}

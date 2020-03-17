import React, { useState } from "react";
import { Button } from 'antd';
import { Localized, lang } from "src/i18n";
import { AddInstanceModal } from "src/pages/Resources/Instance/AddInstanceModal";

const root = lang.resources.instance.add;

interface Props {
  reload: () => void;
}

export const AddInstanceButton: React.FC<Props> = ({ reload }) => {

  const [modalShown, setModalShown] = useState(false);

  return (
    <>
      <Button type="primary" onClick={() => setModalShown(true)}>
        <Localized id={root.button} />
      </Button>
      <AddInstanceModal
        onCreated={reload}
        visible={modalShown}
        close={() => setModalShown(false)}
      />
    </>
  )
};

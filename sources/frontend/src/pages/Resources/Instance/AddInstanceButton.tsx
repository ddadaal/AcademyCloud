import React, { useState } from "react";
import { Button } from 'antd';
import { Localized, lang } from "src/i18n";
import { AddInstanceModal } from "src/pages/Resources/Instance/AddInstanceModal";

const root = lang.resources.instance.add;


export const AddInstanceButton: React.FC = (props) => {

  const [modalShown, setModalShown] = useState(true);

  return (
    <>
      <Button type="primary" onClick={() => setModalShown(true)}>
        <Localized id={root.button} />
      </Button>
      <AddInstanceModal
        visible={modalShown}
        close={() => setModalShown(false)}
      />
    </>
  )
};

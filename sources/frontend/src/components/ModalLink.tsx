import React from "react";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { Modal } from "antd";

export const ModalLink: React.FC<{
  modalTitle: React.ReactNode;
  modalContent: React.ReactNode;
}> = ({ children, modalTitle, modalContent }) => {

  const [api, contextHolder] = Modal.useModal();

  return (
    <>
      {contextHolder}
      <a onClick={() => api.info({
        icon: null,
        title: <TitleText>{modalTitle}</TitleText>,
        content: modalContent
      })}>
        {children}
      </a>
    </>
  );
}

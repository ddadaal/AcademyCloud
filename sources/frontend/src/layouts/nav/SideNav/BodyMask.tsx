import React, { useCallback } from "react";

import styled from "styled-components";
import { useStore } from "simstate";
import { NavStore } from "../NavStore";

interface Props {
  sidebarShown: boolean;
  breakpoint: number;
}

const Mask = styled.div`
  position: absolute;
  left: 0;
  right: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.35);
  z-index:2;

  display: none;

  @media (max-width: ${(props: Props) => props.breakpoint}px) {
    display: ${(props: Props) => props.sidebarShown ? "initial" : "none"};
  }


`;

export default function BodyMask(props: Props) {
  const navStore = useStore(NavStore);

  const onClick = useCallback(() => {
    navStore.setSidebarCollapsed(true);
  }, [navStore]);

  return (
    <Mask
      onClick={onClick}
      breakpoint={props.breakpoint}
      sidebarShown={props.sidebarShown} />
  );
}

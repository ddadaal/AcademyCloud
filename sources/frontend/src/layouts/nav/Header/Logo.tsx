import React, { useCallback } from "react";
import { ReactComponent as LogoSvg } from "src/assets/logo-horizontal.svg";
import { ReactComponent as LogoNoTextSvg } from "src/assets/logo-no-text.svg";
import styled from "styled-components";
import { useStore } from "simstate";
import { UserStore } from "src/stores/UserStore";
import { navigate } from "@reach/router";
import { antdBreakpoints } from "src/layouts/constants";
import MediaQuery, { useMediaQuery } from "react-responsive";


const StyledLogo = styled(LogoSvg)`

  height: 40px;
  width: auto;

  &:hover {
    cursor: pointer;
  }
`;

const StyledNoTextLogo = styled(LogoNoTextSvg)`

  height: 40px;
  width: auto;

  &:hover {
    cursor: pointer;
  }
`;


export const Logo: React.FC = () => {
  const userStore = useStore(UserStore);

  const small = useMediaQuery({ maxWidth: antdBreakpoints.md });

  const jump = useCallback(() => {
    if (userStore.loggedIn) {
      navigate("/resources");
    } else {
      navigate("/");
    }
  }, [userStore.loggedIn]);

  return small ? (
    <StyledNoTextLogo onClick={jump} />
  ) : (
    <StyledLogo onClick={jump} />
  );
};

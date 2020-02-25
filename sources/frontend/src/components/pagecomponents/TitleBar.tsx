import React from "react";
import styled from "styled-components";

interface Props {
  spaceBetween?: boolean;
}

export const TitleBar = styled.div<Props>`
  margin: 8px 0;

  ${({spaceBetween = false}) => !spaceBetween ? "" : `
    display: flex;
    justify-content: space-between;
    align-items: center;
  `}
`;


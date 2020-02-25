import styled from "styled-components";
import { layoutConstants } from "src/layouts/constants";

export const HeaderItemContainer = styled.div`
  :hover {
    background-color: ${layoutConstants.headerIconBackgroundColor};
    cursor: pointer;
  }

  padding-left: 8px;
  padding-right: 8px;
`;


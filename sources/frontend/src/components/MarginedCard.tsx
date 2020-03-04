import styled from "styled-components";
import { Card } from "antd";

interface Props {
  margin?: number;
}

export const MarginedCard = styled(Card)<Props>`
  && {
    margin: ${({ margin = 8}) => margin}px 0;
  }
`;

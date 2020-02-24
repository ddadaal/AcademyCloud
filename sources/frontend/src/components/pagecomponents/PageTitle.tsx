import React from "react";
import styled from 'styled-components';

const Title = styled.h1`
  font-weight: 700;
  font-size: 16px;
`;

export const TitleText: React.FC = ({ children }) => {
  return (
    <Title>{children}</Title>
  );
}


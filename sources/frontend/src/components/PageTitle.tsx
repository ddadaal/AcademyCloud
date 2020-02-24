import React from "react";
import styled from 'styled-components';

const Title = styled.h1`
  margin: 8px 0;

  font-weight: 700;
  font-size: 16px;
`;

export const PageTitle: React.FC = ({ children }) => {
  return (
    <Title>{children}</Title>
  );
}


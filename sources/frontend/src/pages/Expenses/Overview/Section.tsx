import React from "react";
import styled from "styled-components";

interface Props {
  title: React.ReactNode;
  extra: React.ReactNode;
}

const Container = styled.div`
`;

const Title = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
`;

const TitleText = styled.h3`
  font-weight: 500;
`;

export const Section: React.FC<Props> = ({ title, extra, children }) => {

  return (
    <Container>
      <Title>
        <TitleText>
          {title}
        </TitleText>
        {extra}
      </Title>
      {children}
    </Container>
  );
}

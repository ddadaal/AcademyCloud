import React from "react";
import styled from "styled-components";
import { Localized } from "src/i18n";
import { MarginedCard } from "src/components/MarginedCard";

interface Props<T> {
  data: T | undefined;
  children: (value: T) => React.ReactNode;
  title: React.ReactNode;
}

const Title = styled.h3`
  font-weight: 600;
`;

export function StatsCard<T>({ data, children, title }: Props<T>) {
  return (
    <MarginedCard>
      <Title>
        {title}
      </Title>
      {data !== undefined ? children(data) : null}
    </MarginedCard>
  )
}


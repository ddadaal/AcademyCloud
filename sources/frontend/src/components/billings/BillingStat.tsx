import React from "react";
import styled from "styled-components";
import { Localized } from "src/i18n";
import { MarginedCard } from "src/components/MarginedCard";

interface Props<T> {
  data: T | undefined;
  children: (value: T) => React.ReactNode;
  titleId: string;
}

const Title = styled.h3`
  font-weight: 600;
`;

export function BillingStat<T>({ data, children, titleId }: Props<T>) {
  return (
    <MarginedCard>
      <Title>
        <Localized id={titleId} />
      </Title>
      {data ? children(data) : null}
    </MarginedCard>
  )
}


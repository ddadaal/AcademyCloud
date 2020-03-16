import React from "react";
import { Row, Col, Progress, Spin } from "antd";
import { lang, Localized } from "src/i18n";
import { StatsCard } from "src/components/StatCard";
import { useAsync } from "react-async";
import { getApiService } from "src/apis";
import { ResourcesService, GetResourcesUsedAndLimitsResponse } from "src/apis/resources/ResourcesService";
import { Resources } from "src/models/Resources";
import styled from "styled-components";
import { FlexBox } from "src/components/Flexbox";

const root = lang.components.resources;

const service = getApiService(ResourcesService);

const getLimits = () => service.getResourcesUsedAndLimits();

const Centered= styled(FlexBox)`
  align-items: center;
  justify-content: center;
`;

const StatCol = ({ dataKey, data }: {dataKey: keyof Resources; data: GetResourcesUsedAndLimitsResponse | undefined }) => {
  return (
    <Col xs={24} md={8}>
      <StatsCard data={data} title={<Localized id={root[dataKey]} />}>
        {d => (
          <Centered>
            <Progress type="dashboard" percent={d.used[dataKey] / d.allocated[dataKey] * 100}
              format={() => `${d.used[dataKey]} / ${d.allocated[dataKey]}`} />
          </Centered>
        )}

      </StatsCard>
    </Col>
  );
}

export const Limits: React.FC = (props) => {

  const { data, isPending } = useAsync({ promiseFn: getLimits });

  return (
    <Spin spinning={isPending}>
      <Row gutter={16}>
        <StatCol data={data} dataKey="cpu"/>
        <StatCol data={data} dataKey="memory"/>
        <StatCol data={data} dataKey="storage"/>
      </Row>
    </Spin>
  )
};

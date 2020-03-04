import React from "react";
import { RouteComponentProps } from "@reach/router";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { Balance } from "src/pages/Expenses/Overview/Balance";
import { Row, Col, Card } from "antd";

export const Overview: React.FC<RouteComponentProps> = (props) => {
  return (
    <div>
      <TitleBar>
        <TitleText>
          Overview
        </TitleText>
      </TitleBar>
      <Row>
        <Col span={24}>
          <Card>
            <Balance />
          </Card>
        </Col>
      </Row>
    </div>
  );
}

export default Overview;

import React from "react";
import { RouteComponentProps } from "@reach/router";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { Balance } from "src/pages/Expenses/Overview/Balance";
import { RoleSpecific } from "./RoleSpecific";
import { Row, Col } from "antd";


export const Overview: React.FC<RouteComponentProps> = (props) => {
  return (
    <div>
      <TitleBar>
        <TitleText>
          Overview
        </TitleText>
      </TitleBar>
      <Row gutter={16}>
        <Col xs={24} xl={8}>
          <Balance />
        </Col>
        <Col xs={24} xl={16}>
          <RoleSpecific />
        </Col>
      </Row>
    </div>
  );
}

export default Overview;

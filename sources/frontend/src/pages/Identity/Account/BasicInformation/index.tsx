import React from "react";
import { RouteComponentProps } from "@reach/router";
import { PageTitle } from "src/components/PageTitle";
import { ProfileForm } from "src/pages/Identity/Account/BasicInformation/ProfileForm";
import styled from "styled-components";
import { Col, Row } from "antd";
import { ChangePasswordForm } from "src/pages/Identity/Account/BasicInformation/ChangePasswordForm";

const ProfileFormDiv = styled.div`
  max-width: 400px;
`;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default function BasicInformationPage(_: RouteComponentProps) {

  return (
    <div>
      <Row gutter={32}>
        <Col xs={24} md={12}>
          <PageTitle>Basic Information</PageTitle>
          <ProfileFormDiv>
            <ProfileForm />
          </ProfileFormDiv>
        </Col>
        <Col xs={24} md={12}>
          <PageTitle>
            Change Password
          </PageTitle>
          <ProfileFormDiv>
            <ChangePasswordForm />
          </ProfileFormDiv>
        </Col>
      </Row>
    </div>
  );
}


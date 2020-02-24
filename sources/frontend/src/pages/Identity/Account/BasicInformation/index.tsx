import React from "react";
import { RouteComponentProps } from "@reach/router";
import { ProfileForm } from "src/pages/Identity/Account/BasicInformation/ProfileForm";
import styled from "styled-components";
import { Col, Row } from "antd";
import { ChangePasswordForm } from "src/pages/Identity/Account/BasicInformation/ChangePasswordForm";
import { lang, LocalizedString } from "src/i18n";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";

const ProfileFormDiv = styled.div`
  max-width: 400px;
`;

const root = lang.identity.account.basic;

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export default function BasicInformationPage(_: RouteComponentProps) {

  return (
    <div>
      <Row gutter={32}>
        <Col xs={24} md={12}>
          <TitleBar>
            <TitleText>
              <LocalizedString id={root.profile.title} />
            </TitleText>
          </TitleBar>
          <ProfileFormDiv>
            <ProfileForm />
          </ProfileFormDiv>
        </Col>
        <Col xs={24} md={12}>
          <TitleBar>
            <TitleText>
              <LocalizedString id={root.changePassword.title} />
            </TitleText>
          </TitleBar>
          <ProfileFormDiv>
            <ChangePasswordForm />
          </ProfileFormDiv>
        </Col>
      </Row>
    </div>
  );
}


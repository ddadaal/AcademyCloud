import React from "react";
import styled from "styled-components";
// import background from "../../assets/background/1.png";
import { Row, Col, Divider, Button } from "antd";
import { UserStore } from "../../stores/UserStore";
import { layoutConstants, antdBreakpoints } from "../../layouts/constants";
import { useStore } from "simstate";
import { navigate, RouteComponentProps, Link } from "@reach/router";
import CenterContainer from "../../layouts/CenterContainer";
import styles from "./styles.less"
import { LoginForm } from "./LoginForm";
import logo from "src/assets/logo.png";
import banner from "src/assets/banner.png";
import { lang, LocalizedString } from "src/i18n";
import { HomepageLanguageSelector } from "./LanguageSelector";

interface Props {

}

const Container = styled.div`
  display: flex;
  flex-direction: column;
  height: 100vh;
  overflow: auto;

  @media (min-width: ${antdBreakpoints.md}px) {
    background-image: url('https://gw.alipayobjects.com/zos/rmsportal/TVYTbAXWheQpRcWDaDMu.svg');
    background-repeat: no-repeat;
    background-position: center 110px;
    background-size: 100%;
  }
`;

const Content = styled.div`
  flex: 1;
  padding: 32px 0;
  @media (min-width: ${antdBreakpoints.md}) {
    padding: 32px 0 24px;
  }
`;

const Top = styled.div`
  text-align: center;
`;

const Header = styled.div`
  /* height: 44px;
  line-height: 44px; */
  a {
    text-decoration: none;
  }
`;

const LoginFormSection = styled.div`
  max-width: 400px;
  margin-left: auto;
  margin-right: auto;
  margin-top: 32px;
`;

export const HomePage: React.FC<RouteComponentProps> = () => {
  return (
    <Container>
      <Content>
        <Top>
          <Header>
            <Link to="/">
              <img alt="logo" className={styles.logo} src={banner} />
            </Link>
          </Header>
          <HomepageLanguageSelector />
        </Top>
        <LoginFormSection>
          <LoginForm />
        </LoginFormSection>
      </Content>
    </Container >
  );
}

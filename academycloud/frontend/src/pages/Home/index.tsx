import React from "react";
import styled from "styled-components";
import { antdBreakpoints } from "../../layouts/constants";
import { RouteComponentProps, Link, Redirect } from "@reach/router";
import { LoginForm } from "./LoginForm";
import logo from "src/assets/logo-horizontal.svg";
import { HomepageLanguageSelector } from "./LanguageSelector";
import { UserStore } from "src/stores/UserStore";
import { useStore } from "simstate";


const Container = styled.div`
  display: flex;
  flex-direction: column;
  height: 100vh;
  overflow: auto;

  @media (min-width: ${antdBreakpoints.md}px) {
    background-image: url('https://gw.alipayobjects.com/zos/rmsportal/TVYTbAXWheQpRcWDaDMu.svg');
    background-position: center 110px;
    background-size: 100%;
  }
`;

const Center = styled.div`
  padding: 32px;

  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
`;

const Top = styled.div`
  text-align: center;
`;

const Header = styled.div`
  a {
    text-decoration: none;
  }
`;

const Content = styled.div`
  max-width: 500px;

  transform: translateY(-20%);
`;

const LoginFormSection = styled.div`
  max-width: 400px;
  margin-left: auto;
  margin-right: auto;
  margin-top: 32px;
`;


export const HomePage: React.FC<RouteComponentProps> = () => {

  const userStore = useStore(UserStore);
  if (userStore.loggedIn) {
    return <Redirect noThrow={true} to="/resources"/>
  }

  return (
    <Container>
      <Center>
        <Content>
          <Top>
            <Header>
              <Link to="/">
                {/* <StyledLogo /> */}
                {/* {React.createElement(banner)} */}
                <img width={"100%"} alt="logo" src={logo} />
              </Link>
            </Header>
            <HomepageLanguageSelector />
          </Top>
          <LoginFormSection>
            <LoginForm />
          </LoginFormSection>
        </Content>
      </Center>
    </Container >
  );
}

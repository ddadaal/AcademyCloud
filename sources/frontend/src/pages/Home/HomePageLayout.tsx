import React from "react";
import styled from "styled-components";
import { Link } from "@reach/router";
import { HomepageLanguageSelector } from "src/pages/Home/LanguageSelector";
import { Footer } from "src/components/Footer";
import logo from "src/assets/logo-horizontal.svg";
import { Button, Radio } from "antd";
import { PageIndicator } from "src/pages/Home/PageIndicator";

const Container = styled.div`
  display: flex;
  flex-direction: column;
  height: 100vh;
  overflow: auto;

  background-image: url('https://gw.alipayobjects.com/zos/rmsportal/TVYTbAXWheQpRcWDaDMu.svg');
  background-position: center 110px;
  background-size: 100%;
`;

const Center = styled.div`
  padding: 32px;

  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;

`;

const RowCenter = styled.div`
  text-align: center;
`;

const Header = styled.div`
  a {
    text-decoration: none;
  }
`;

const Content = styled.div`
  max-width: 600px;

  transform: translateY(-10%);
`;

const FormSection = styled.div`
  max-width: 400px;
  margin-left: auto;
  margin-right: auto;
  margin-top: 32px;
`;

export const HomePageLayout: React.FC = ({ children }) => {
  return (
    <div>
      <Container>
        <Center>
          <Content>
            <Header>
              <Link to="/">
                <img width={"100%"} alt="logo" src={logo} />
              </Link>
            </Header>
            <RowCenter>
              <PageIndicator />
            </RowCenter>
            <FormSection>
              {children}
            </FormSection>
            <RowCenter>
              <HomepageLanguageSelector />
            </RowCenter>
          </Content>
        </Center>
      </Container >
      <Footer />
    </div>
  )
};

export const FormButton = styled(Button)`
  width: 100%;
`;

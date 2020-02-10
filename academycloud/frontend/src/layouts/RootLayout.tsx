import React from "react";
import { Header } from "src/layouts/nav/Header";
import styled from "styled-components";
import { layoutConstants } from "./constants";
import { Layout } from "antd";
import { SideNav } from "./nav/SideNav";
// import { Footer } from "@/components/Footer";

const CenterContent = styled.div`
  position: relative;
  margin-left: auto;
  margin-right: auto;
  width: auto;
  max-width: ${layoutConstants.maxWidth}px;
`;

const RootLayout: React.FunctionComponent = (props) => {
  return (
    <Layout>
      <Header />
      <Layout>
        <SideNav />
        {props.children}
      </Layout>
      {/* <Footer /> */}
    </Layout>
  )
};

export default RootLayout;

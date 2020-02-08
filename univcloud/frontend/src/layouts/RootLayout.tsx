import React from "react";
import { Header } from "src/components/Header";
import styled from "styled-components";
import { layoutConstants } from "./constants";
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
    <div>
      <Header />
      {props.children}
      {/* <Footer /> */}
    </div>
  )
};

export default RootLayout;

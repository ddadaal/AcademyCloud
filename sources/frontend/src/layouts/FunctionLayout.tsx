import React from "react";
import { Layout as AntdLayout } from "antd";
import styled from "styled-components";
import { layoutConstants } from "./constants";
import { BreadcrumbNav } from "./nav/BreadcrumbNav";

const { Content } = AntdLayout;

const Layout = styled(AntdLayout)`
  @media (min-width: ${layoutConstants.paddingBreakpoint}px) {
   padding: 0 8px 8px;
  }
`;

const StyledContent = styled(Content)`
  padding: 24px;
  margin: 8px;
  // fix min-height is set to 0 at first start
  && {
    min-height: 800px;
  }
  background-color: #FFFFFF;
`;

const StyledBreadcrumbNav = styled(BreadcrumbNav)`
  margin: 4px 4px 4px 4px !important;

  padding-left: 4px;

  @media (max-width: ${layoutConstants.paddingBreakpoint}px) {
  }

`;


const FunctionLayout: React.FC = (props) => {
  return (
    <Layout>
      <StyledBreadcrumbNav />
      <StyledContent >
        {props.children}
      </StyledContent>
    </Layout>
  );

}

export default FunctionLayout;

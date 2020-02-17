import React from "react";
import { useLocation } from "src/utils/useLocation";
import { Radio } from "antd";
import { navigate } from "@reach/router";

export const PageIndicator: React.FC = () => {
  const location = useLocation();

  return (
    <Radio.Group value={location.pathname}>
      <Radio.Button onClick={() => navigate("/")} value="/">
        Login
      </Radio.Button>
      <Radio.Button onClick={() => navigate("/register")} value="/register">
        Register
      </Radio.Button>
    </Radio.Group>
  );
};

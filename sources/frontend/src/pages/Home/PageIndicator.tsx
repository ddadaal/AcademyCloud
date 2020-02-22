import React from "react";
import { useLocation } from "src/utils/useLocation";
import { Radio } from "antd";
import { navigate } from "@reach/router";
import { lang, LocalizedString } from "src/i18n";
const root = lang.homepage.pageIndicator;

export const PageIndicator: React.FC = () => {
  const location = useLocation();

  return (
    <Radio.Group value={location.pathname}>
      <Radio.Button onClick={() => navigate("/")} value="/">
        <LocalizedString id={root.login} />
      </Radio.Button>
      <Radio.Button onClick={() => navigate("/register")} value="/register">
        <LocalizedString id={root.register} />
      </Radio.Button>
    </Radio.Group>
  );
};

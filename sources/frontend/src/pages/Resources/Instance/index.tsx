import React from "react";
import { RouteComponentProps } from "@reach/router";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { lang, Localized } from "src/i18n";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { InstanceManagementTable } from "src/pages/Resources/Instance/InstanceManagementTable";
import { ClickableA } from "src/components/ClickableA";
import { useRefreshToken } from "src/utils/refreshToken";
import { AddInstanceButton } from "src/pages/Resources/Instance/AddInstanceButton";
import { Divider } from "antd";

const root = lang.resources.instance;

const InstancePage: React.FC<RouteComponentProps> = (props) => {

  const [token, refresh] = useRefreshToken();

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText>
          <Localized id={root.title} />
        </TitleText>
        <span>
          <AddInstanceButton />
          <Divider type="vertical" />
          <ClickableA onClick={refresh}>
            <Localized id={root.refresh} />
          </ClickableA>
        </span>
      </TitleBar>
      <InstanceManagementTable refreshToken={token} />
    </div>
  )
};

export default InstancePage;

import React from "react";
import { lang, Localized } from "src/i18n";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { ClickableA } from "src/components/ClickableA";
import { useRefreshToken } from "src/utils/refreshToken";
import { VolumeTable } from "src/pages/Resources/Volume/VolumeTable";

const root = lang.resources.volume;

export const Volume: React.FC = (props) => {

  const [token, refresh] = useRefreshToken();

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText>
          <Localized id={root.title} />
        </TitleText>
        <span>
          <ClickableA onClick={refresh}>
            <Localized id={root.refresh} />
          </ClickableA>
        </span>
      </TitleBar>
      <VolumeTable refreshToken={token} />
    </div>
  );
};

export default Volume;

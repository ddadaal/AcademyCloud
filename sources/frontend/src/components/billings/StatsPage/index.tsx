import React, { useCallback } from "react";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { Stats } from "src/components/billings/StatsPage/Stats";
import { HistoryTable } from "src/components/billings/StatsPage/HistoryTable";
import { BillSubjectType, BillType } from "src/models/Billings";
import { lang, Localized } from "src/i18n";
import styled from "styled-components";
import { Divider } from "antd";
import { useRefreshToken } from "src/utils/refreshToken";
import { ClickableA } from "src/components/ClickableA";

interface Props {
  billType: BillType;
  billSubjectType: BillSubjectType;
  id: string;
  name: string;
}

const root = lang.components.billings;

const HistoryCycleTitle = styled.h3`
  font-size: 16px;
  margin: 16px 0;
  font-weight: 600;
`;

export const StatsPage: React.FC<Props> = (props) => {

  const [token, reload] = useRefreshToken();

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText>
          <Localized id={root.stats.title} replacements={[
            <Localized key="subjectType" id={root.subjectType[props.billSubjectType]} />,
            props.name,
            <Localized key="billType" id={root[props.billType]} />,
          ]} />
        </TitleText>
        <ClickableA onClick={reload}><Localized id={root.refresh} /></ClickableA>
      </TitleBar>
      <div>
        <Stats refreshToken={token} {...props} />
      </div>
      <Divider />
      <div>
        <HistoryCycleTitle>
          <Localized id={root.stats.history} />
        </HistoryCycleTitle>
        <HistoryTable refreshToken={token} {...props} />
      </div>
    </div >
  )
}

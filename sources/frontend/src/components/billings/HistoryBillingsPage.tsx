import React, { useCallback } from "react";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { Localized, lang, RecursiveLocalized } from "src/i18n";
import { ClickableA } from "src/components/ClickableA";
import { RouteComponentProps, Link } from "@reach/router";
import { StepBackwardOutlined } from "@ant-design/icons";
import { Button, Divider } from "antd";
import { HistoryBillingsTable } from "src/components/billings/HistoryBillingsTable";
import { useAsync } from "react-async";
import { FlexBox } from "src/components/Flexbox";
import {
  BillSubjectType,
  BillType,
  HistoryAllocatedBilling, HistoryUsedBilling
} from "../../models/Billings";

type Billing = HistoryUsedBilling | HistoryAllocatedBilling;

interface Props {
  billType: BillType;
  id: string;
  subjectType: BillSubjectType;
  getData: (id: string) => Promise<Billing[]>;
  location: Location;
}

const root = lang.components.billings;

export const HistoryBillingsPage: React.FC<Props> = ({ getData, id, billType, subjectType, location }) => {

  // just parse the first arg and use it as the name
  const name = location?.search?.replace?.(/^.*?=/, '') as string;

  const promiseFn = useCallback(() => (
    getData(id)
  ), [getData, id]);

  const { data, isPending, reload } = useAsync({ promiseFn });

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <FlexBox>
          <TitleText>
            <Button>
              <Link to=".."><StepBackwardOutlined /></Link>
            </Button>
            <Divider type="vertical" />
            <Localized id={root.history} replacements={[
              <Localized key={"subjectType"} id={root.subjectType[subjectType]} />,
              name || id,
              <Localized key="billType" id={root[billType]} />
            ]} />
          </TitleText>
        </FlexBox>
        <ClickableA onClick={reload}><Localized id={root.refresh} /></ClickableA>
      </TitleBar>
      <HistoryBillingsTable type={billType} data={data} loading={isPending} />
    </div>
  );
}

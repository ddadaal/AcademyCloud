import React, { useMemo } from "react";
import { TitleText } from "src/components/pagecomponents/TitleText";
import { TitleBar } from "src/components/pagecomponents/TitleBar";
import { Localized, lang, RecursiveLocalized } from "src/i18n";
import { ClickableA } from "src/components/ClickableA";
import { RouteComponentProps } from "@reach/router";
import { useAsync } from "react-async";
import { CurrentBillingsTable } from "src/components/billings/CurrentBillingsTable";
import { BillSubjectType, BillType, CurrentAllocatedBilling, CurrentUsedBilling } from "../../models/Billings";

const root = lang.components.billings;

type Billing = CurrentUsedBilling | CurrentAllocatedBilling;

interface Props extends RouteComponentProps {
  subjectType: BillSubjectType;
  billType: BillType;
  promiseFn: () => Promise<Billing[]>;
}

export const CurrentBillingsPage: React.FC<Props> = ({ promiseFn, subjectType, billType }) => {

  const { data, isPending, reload } = useAsync({ promiseFn });

  const processedData = useMemo(() => {
    return data?.map(x => ({ ...x, historyLink: `./${x.subjectId}?name=${x.subjectName}` }))
  }, [data])

  return (
    <div>
      <TitleBar spaceBetween={true}>
        <TitleText>
          <Localized id={root.current} replacements={[
            <Localized key={"subjectType"} id={root.subjectType[subjectType]}/>,
            <Localized key="billType" id={root[billType]}/>
          ]}/>
        </TitleText>
        <ClickableA onClick={reload}><Localized id={root.refresh}/></ClickableA>
      </TitleBar>
      <CurrentBillingsTable subjectType={subjectType} type={billType} data={processedData} loading={isPending}/>
    </div>
  );
}

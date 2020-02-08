import React, { useCallback } from "react";
import { Radio } from "antd";
import { I18nStore } from "src/i18n";
import { useStore } from "simstate";
import { allLanguages } from "src/i18n/definition";
import { RadioChangeEvent } from "antd/lib/radio";

export function HomepageLanguageSelector() {

  const { currentLanguage, changeLanguage } = useStore(I18nStore);

  const onChange = useCallback((e: RadioChangeEvent) => {
    changeLanguage(e.target.value);
  }, []);

  return (
    <Radio.Group value={currentLanguage.metadata.id} onChange={onChange}>
      {
        allLanguages
          .map((x) =>
            <Radio.Button value={x.metadata.id} key={x.metadata.id}>
              {x.metadata.name}
            </Radio.Button>
          )
      }
    </Radio.Group>
  )
}

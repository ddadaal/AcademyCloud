import React from "react";
import { Spin } from "antd";
import FunctionLayout from "src/layouts/FunctionLayout";

export default function PageLoading() {
  return (
    <Spin spinning={true}>
      <FunctionLayout />
    </Spin>
  );
}

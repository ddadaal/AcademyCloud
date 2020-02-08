const { override, fixBabelImports, addLessLoader, addBabelPlugin, addWebpackPlugin } = require('customize-cra');
const AntdDayjsWebpackPlugin = require('antd-dayjs-webpack-plugin');

module.exports = override(
  fixBabelImports('import', {
    libraryName: 'antd',
    libraryDirectory: 'es',
    style: true,
  }),
  addLessLoader({
    javascriptEnabled: true,
  }),
  addBabelPlugin(
    "babel-plugin-styled-components"
  ),
  // addBabelPlugin(
  //   ["babel-plugin-root-import", {
  //     root:
  //   }
  //   ]
  // ),
  addWebpackPlugin(new AntdDayjsWebpackPlugin())
);

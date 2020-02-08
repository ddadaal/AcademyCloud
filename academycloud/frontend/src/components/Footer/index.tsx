import React from "react";
import styled from "styled-components";
import { Link } from "@reach/router";
import "./footer.less";
import logo from "@/assets/logo.png";

export interface FooterProps {
  isMobile?: boolean;
  id?: string;
}

const dataSource = [
  {
    title: "联系我",
    content: [
      "GitHub"
    ],
    contentLink: [
      "https://github.com/ddadaal"
    ]
  },
  {
    title: "相关产品",
    content: [
      "ChainStore - 基于区块链的分布式存储解决方案",
      "FinBrain - 基于机器学习的大类资产管理系统"
    ],
    contentLink: [
      "https://github.com/NJUChainStore/ChainStore",
      "https://github.com/FinTechNJU/FinBrain"
    ],
  },
];

export class Footer extends React.Component<FooterProps, {}> {

  static defaultProps = {
    className: "footer1",
    id: "footer_1_0",
    isMobile: false,
  };

  getLiChildren = (data, i) => {
    const links = data.contentLink;
    const content = data.content
      .map((item, ii) => {
        const cItem = item.trim();
        const isImg = cItem.match(/\.(jpg|png|svg|bmp|jpeg)$/i);
        const link = links[ii];
        const imgContent = isImg ? <img src={cItem} width="100%" /> : cItem;
        return (<li className={isImg ? "icon" : ""} key={ii}>
          {link.startsWith("/")
            ? <Link to={link}>{imgContent}</Link>
            : <a href={link} target="_blank" rel="noopener noreferrer">{imgContent}</a>
          }
        </li>);
      });
    return (<li className={data.className} key={i} id={`${this.props.id}-block${i}`}>
      <h2>{data.title}</h2>
      <ul>
        {content}
      </ul>
    </li>);
  }

  render() {
    const props = { ...this.props };
    delete props.isMobile;

    const liChildrenToRender = dataSource.map(this.getLiChildren);
    return (
      <div {...props}>

        <ul>
          <li key="logo" id={`${props.id}-logo`}>
            <LogoContainer>
              <img src={logo} />
            </LogoContainer>
            <p>基于区块链的论文分享平台；论文交流，版权保证</p>
          </li>
          {liChildrenToRender}
        </ul>
        <span className="copyright">
          2019
        </span>
      </div>
    );
  }
}

export const LogoContainer = styled.div`
  span {
    color: #ffffff;
    font-weight: bold;
    font-size: 21px;
  }
  img {
    margin-right: 8px;
    width: 80px;
    height: 40px;
  }
`;

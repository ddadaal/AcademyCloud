import React from "react";
import styled from "styled-components";
import { Link } from "@reach/router";
import "./footer.less";
import logo from "src/assets/logo-horizontal.svg";
import { lang, LocalizedString } from "src/i18n";

export interface FooterProps {
  isMobile?: boolean;
  id?: string;
}
const root = lang.footer;

const dataSource = [
  {
    title: root.contact.title,
    content: [
      root.contact.github,
      root.contact.website,
      root.contact.linkedin,
    ],
    contentLink: [
      "https://github.com/ddadaal",
      "https://ddadaal.me",
      "https://linkedin.com/chenjunda",
    ],
  },
  {
    title: root.moreProducts.title,
    content: [
      root.moreProducts.chainstore,
      root.moreProducts.chainpaper,
      root.moreProducts.aplusquant,
      root.moreProducts.tagx00,
      root.moreProducts.lightx00,
    ],
    contentLink: [
      "https://github.com/NJUChainStore/ChainStore",
      "https://github.com/ddadaal/ChainPaper-Frontend",
      "https://github.com/FinTechNJU/FinBrain",
      "https://github.com/trapx00/Tagx00",
      "https://github.com/trapx00/Lightx00",
    ],
  },
];


export class Footer extends React.Component<FooterProps, {}> {

  static defaultProps = {
    className: "footer1",
    id: "footer_1_0",
    isMobile: false,
  };

  getLiChildren = (data: typeof dataSource[0], i: number) => {
    const links = data.contentLink;
    const content = data.content
      .map((item, ii) => {
        const cItem = <LocalizedString id={item} />;
        const link = links[ii];
        return (
          <li key={ii}>
            {link.startsWith("/")
              ? <Link to={link}>{cItem}</Link>
              : <a href={link} target="_blank" rel="noopener noreferrer">{cItem}</a>
            }
          </li>
        );
      });
    return (
      <li key={i} id={`${this.props.id}-block${i}`}>
        <h2><LocalizedString id={data.title} /></h2>
        <ul>
          {content}
        </ul>
      </li>
    );
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
            <p><LocalizedString id={root.description}/> </p>
          </li>
          {liChildrenToRender}
        </ul>
        <p className="copyright">
          {new Date().getFullYear()} | <LocalizedString id={root.copyright.madeWithLove}/>
        </p>
      </div>
    );
  }
}


export const LogoContainer = styled.div`
  img {
    width: auto;
    height: 40px;
    margin-bottom: 12px;
  }
`;

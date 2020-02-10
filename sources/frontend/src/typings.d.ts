declare module "*.less" {
  const module: any;
  export = module;
}

declare type IconType = React.ForwardRefExoticComponent<AntdIconProps & React.RefAttributes<HTMLSpanElement>>;

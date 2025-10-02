// Copyright © Spatial Corporation. All rights reserved.

import { DetailedHTMLProps, FC, HTMLAttributes, ReactNode } from "react";

/**
 * Create a new layout component.
 * @param props Configurable options for the component.
 * @returns A layout component.
 */
export const Layout: FC<DetailedHTMLProps<HTMLAttributes<HTMLDivElement>, HTMLDivElement>> = (props): ReactNode => {
  return <div {...props} />;
};

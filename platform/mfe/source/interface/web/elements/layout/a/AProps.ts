// Copyright © Spatial. All rights reserved.

import { MouseEventHandler } from "react";
import { ElementProps } from "../..";

/**
 * Configurable options for an anchor element.
 */
export type AProps = ElementProps & {
  /**
   * An optional hypertext reference.
   */
  href?: string;

  /**
   * An optional mouse event handler.
   */
  onClick?: MouseEventHandler<HTMLAnchorElement>;

  /**
   * Whether or not the link is external.
   */
  external?: boolean;

  /**
   * Whether or not to highlight the link.
   */
  highlight?: boolean;
};

// Copyright © Spatial Corporation. All rights reserved.

import { MouseEventHandler } from "react";
import { ElementProps } from "../..";

/**
 * Configurable options for a toast action element.
 */
export type ToastActionProps = ElementProps & {
  /**
   * Whether or not to render the action as its child.
   */
  asChild?: boolean;

  /**
   * Alternative text for the action.
   */
  altText: string;

  /**
   * An optional click event handler.
   */
  onClick?: MouseEventHandler;
};

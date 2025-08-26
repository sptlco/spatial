// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a dialog element.
 */
export type DialogProps = ElementProps & {
  /**
   * Whether or not the dialog is open.
   */
  open?: boolean;

  /**
   * An optional open change event handler.
   * @param open Whether or not the dialog is open.
   */
  onOpenChange?: (open: boolean) => void;
};

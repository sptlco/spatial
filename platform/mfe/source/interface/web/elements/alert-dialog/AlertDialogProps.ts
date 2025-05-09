// Copyright © Spatial. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for an alert dialog element.
 */
export type AlertDialogProps = ElementProps & {
  /**
   * Whether or not the dialog is open.
   */
  open?: boolean;

  /**
   * An optional open change event handler.
   * @param open Whether or not the alert is open.
   */
  onOpenChange?: (open: boolean) => void;
};

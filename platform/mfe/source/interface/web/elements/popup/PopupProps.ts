// Copyright © Spatial. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a popup element.
 */
export type PopupProps = ElementProps & {
  /**
   * Whether or not the popup is open.
   */
  open?: boolean;

  /**
   * An optional change event handler.
   * @param open Whether or not the popup is open.
   */
  onChange?: (open: boolean) => void;
};

// Copyright © Spatial. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a sheet element.
 */
export type SheetProps = ElementProps & {
  /**
   * Whether or not the sheet is open.
   */
  open?: boolean;

  /**
   * An optional open change event handler.
   * @param open Whether or not the sheet is open.
   */
  onOpenChange?: (open: boolean) => void;
};

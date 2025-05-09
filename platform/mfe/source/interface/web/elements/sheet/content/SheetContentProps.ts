// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a sheet content element.
 */
export type SheetContentProps = ElementProps & {
  /**
   * The side to display the sheet content on.
   */
  side?: "right" | "left" | "top" | "bottom";
};

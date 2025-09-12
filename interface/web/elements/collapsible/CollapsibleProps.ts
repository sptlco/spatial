// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a collapsible element.
 */
export type CollapsibleProps = ElementProps & {
  /**
   * Whether or not the element is expanded by default.
   */
  defaultExpanded?: boolean;
};

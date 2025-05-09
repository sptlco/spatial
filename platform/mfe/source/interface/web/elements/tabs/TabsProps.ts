// Copyright © Spatial. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a tabs element.
 */
export type TabsProps = ElementProps & {
  /**
   * The value of the tabs element.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The value of the tabs element.
   */
  onChange?: (value: string) => void;

  /**
   * The orientation of the element.
   */
  orientation?: "horizontal" | "vertical";
};

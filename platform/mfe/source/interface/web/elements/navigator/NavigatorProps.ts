// Copyright © Spatial. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a navigator element.
 */
export type NavigatorProps = ElementProps & {
  /**
   * The navigator's orientation.
   */
  orientation?: "horizontal" | "vertical";

  /**
   * The navigator's default value.
   */
  defaultValue?: string;

  /**
   * The navigator's value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The navigator's value.
   */
  onChange?: (value: string) => void;
};

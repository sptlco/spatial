// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a toggle element.
 */
export type ToggleProps = ElementProps & {
  /**
   * Whether or not the element is pressed.
   */
  pressed?: boolean;

  /**
   * An optional change event handler.
   * @param pressed Whether or not the element is pressed.
   */
  onChange?: (pressed: boolean) => void;

  /**
   * Whether or not the element is disabled.
   */
  disabled?: boolean;
};

// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../../../..";

/**
 * Configurable options for a dropdown radio group element.
 */
export type DropdownRadioGroupProps = ElementProps & {
  /**
   * The group's current value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The new value.
   */
  onChange?: (value: string) => void;
};

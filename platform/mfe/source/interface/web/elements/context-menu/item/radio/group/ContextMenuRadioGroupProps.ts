// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../../../..";

/**
 * Configurable options for a context menu radio group element.
 */
export type ContextMenuRadioGroupProps = ElementProps & {
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

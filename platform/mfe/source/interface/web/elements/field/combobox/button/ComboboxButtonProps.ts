// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../../..";

/**
 * Configurable options for a combobox button element.
 */
export type ComboboxButtonProps = ElementProps & {
  /**
   * The button's value.
   */
  value?: string | null;

  /**
   * The button's placeholder.
   */
  placeholder: string;
};

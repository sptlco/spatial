// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../../../..";

/**
 * Configurable options for a combobox options element.
 */
export type ComboboxOptionsProps = ElementProps & {
  /**
   * A search query.
   */
  query: string;

  /**
   * A list of options.
   */
  options: IComboboxOption[];
};

/**
 * A combobox option.
 */
export interface IComboboxOption {
  /**
   * The option's key.
   */
  key: string;

  /**
   * The option's value.
   */
  value: string;
}

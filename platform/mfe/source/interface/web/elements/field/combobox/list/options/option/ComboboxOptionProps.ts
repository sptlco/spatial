// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps, IComboboxOption } from "../../../../..";

/**
 * Configurable options for a combobox option element.
 */
export type ComboboxOptionProps = ElementProps & {
  /**
   * The option's data.
   */
  data: IComboboxOption;
};

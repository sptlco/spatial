// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps, IComboboxOption } from "../../..";

/**
 * Configurable options for a combobox list element.
 */
export type ComboboxListProps = ElementProps & {
  /**
   * An optional search placeholder.
   */
  searchPlaceholder?: string;

  /**
   * A search query.
   */
  query: string;

  /**
   * A function that sets the current search query.
   * @param value A query value.
   */
  setQuery: (value: React.SetStateAction<string>) => void;

  /**
   * A list of options.
   */
  options: IComboboxOption[];
};

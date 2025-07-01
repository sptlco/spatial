// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../../../..";

/**
 * Configurable options for a combobox search element.
 */
export type ComboboxSearchProps = ElementProps & {
  /**
   * An optional placeholder value.
   */
  placeholder?: string;

  /**
   * A search query.
   */
  query: string;

  /**
   * A function that sets the current search query.
   * @param value A query value.
   */
  setQuery: (value: React.SetStateAction<string>) => void;
};

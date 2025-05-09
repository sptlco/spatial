// Copyright © Spatial. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a command input element.
 */
export type CommandInputProps = ElementProps & {
  /**
   * The input's value.
   */
  value?: string;

  /**
   * An optional placeholder value.
   */
  placeholder?: string;

  /**
   * An optional change event handler.
   * @param search A search query.
   */
  onChange?: (search: string) => void;

  /**
   * Whether or not the field is disabled.
   */
  disabled?: boolean;
};

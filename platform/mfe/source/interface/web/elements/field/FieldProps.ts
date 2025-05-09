// Copyright © Spatial. All rights reserved.

import {ElementProps, Node} from "..";

/**
 * Configurable options for a field element.
 */
export type FieldProps = ElementProps & {
  /**
   * The name of the field.
   */
  name?: string;

  /**
   * Whether or not the field is disabled.
   */
  disabled?: boolean;

  /**
   * Whether or not to automatically focus the field.
   */
  autoFocus?: boolean;

  /**
   * An optional label for the field.
   */
  label?: Node;

  /**
   * An optional description for the field.
   */
  description?: Node;
};

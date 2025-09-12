// Copyright © Spatial Corporation. All rights reserved.

import { FieldProps, RadioOrientation } from "..";

/**
 * Configurable options for a radio element.
 */
export type RadioProps = FieldProps & {
  /**
   * The radio's value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The radio's value.
   */
  onChange?: (value: string) => void;

  /**
   * The radio's orientation.
   */
  orientation?: RadioOrientation;
};

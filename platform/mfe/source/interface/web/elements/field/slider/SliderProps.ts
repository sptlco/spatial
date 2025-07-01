// Copyright © Spatial Corporation. All rights reserved.

import { FieldProps } from "..";

/**
 * Configurable options for a slider element.
 */
export type SliderProps = FieldProps & {
  /**
   * The slider's value.
   */
  value?: number[];

  /**
   * An optional change event handler.
   * @param value The slider's value.
   */
  onChange?: (value: number[]) => void;

  /**
   * An optional commit event handler.
   * @param value The slider's value.
   */
  onCommit?: (value: number[]) => void;

  /**
   * The slider's minimum value.
   */
  min?: number;

  /**
   * The slider's maximum value.
   */
  max?: number;

  /**
   * The slider's step.
   */
  step?: number;

  /**
   * The slider's orientation.
   */
  orientation?: "horizontal" | "vertical";
};

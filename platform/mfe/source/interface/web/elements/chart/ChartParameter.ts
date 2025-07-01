// Copyright © Spatial Corporation. All rights reserved.

/**
 * A data parameter.
 */
export type ChartParameter = {
  /**
   * The parameter's key value.
   */
  key: string;

  /**
   * The name of the parameter.
   */
  name: string;

  /**
   * The parameter's color.
   */
  color: string;

  /**
   * The parameter's unit.
   */
  unit?: string | number;
};

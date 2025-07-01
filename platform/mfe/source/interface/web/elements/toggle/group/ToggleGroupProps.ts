// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a single-item toggle group.
 */
export interface ToggleGroupSingleProps extends ToggleGroupProps {
  /**
   * The group's type.
   */
  type: "single";

  /**
   * The group's value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The group's value.
   */
  onChange?: (value: string) => void;
}

/**
 * Configurable options for a multiple-item toggle group.
 */
export interface ToggleGroupMultipleProps extends ToggleGroupProps {
  /**
   * The group's type.
   */
  type: "multiple";

  /**
   * The group's value.
   */
  value?: string[];

  /**
   * An optional change event handler.
   * @param value The group's value.
   */
  onChange?: (value: string[]) => void;
}

/**
 * Configurable options for a toggle group element.
 */
export interface ToggleGroupProps extends ElementProps {
  /**
   * The group's orientation.
   */
  orientation?: "horizontal" | "vertical";

  /**
   * Whether or not the group is disabled.
   */
  disabled?: boolean;
}

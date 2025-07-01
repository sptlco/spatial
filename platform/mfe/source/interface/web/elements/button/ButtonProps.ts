// Copyright © Spatial Corporation. All rights reserved.

import { MouseEventHandler } from "react";
import { ElementProps } from "../ElementProps";
import { ButtonIntent } from "./ButtonIntent";

/**
 * Configurable options for a button element.
 */
export type ButtonProps = ElementProps & {
  /**
   * The button's type.
   */
  type?: "button" | "submit" | "reset";

  /**
   * Whether or not the button is disabled.
   */
  disabled?: boolean;

  /**
   * Whether or not the button is loading.
   */
  loading?: boolean;

  /**
   * The button's intent.
   */
  intent?: ButtonIntent;

  /**
   * The button's click event handler.
   */
  onClick?: MouseEventHandler<HTMLButtonElement>;

  /**
   * An optional direction.
   */
  direction?: "forward" | "backward";

  /**
   * The size of the button.
   */
  size?: "large" | "small";

  /**
   * The roundness of the button.
   */
  roundness?: "round" | "pill";
};

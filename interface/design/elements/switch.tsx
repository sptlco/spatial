// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import * as Primitive from "@radix-ui/react-switch";
import { clsx } from "clsx";

/**
 * A control that allows the user to toggle between on and off.
 */
export const Switch = {
  /**
   * Contains all the parts of a switch.
   */
  Root: createElement<typeof Primitive.Root>((props, ref) => <Primitive.Root {...props} ref={ref} />),

  /**
   * The thumb that is used to visually indicate whether the switch is on or off.
   */
  Thumb: createElement<typeof Primitive.Thumb>((props, ref) => <Primitive.Thumb {...props} ref={ref} />)
};

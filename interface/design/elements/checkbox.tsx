// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import * as Primitive from "@radix-ui/react-checkbox";
import { clsx } from "clsx";
import { cva } from "cva";

/**
 * A control that allows the user to toggle between checked and not checked.
 */
export const Checkbox = createElement<typeof Primitive.Root>((props, ref) => (
  <Primitive.Root {...props} ref={ref}>
    <Primitive.Indicator />
  </Primitive.Root>
));

// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import * as Primitive from "@radix-ui/react-toggle-group";
import { clsx } from "clsx";

/**
 * A set of two-state buttons that can either be on or off.
 */
export const ToggleGroup = {
  /**
   * Contains all the parts of a toggle group.
   */
  Root: createElement<typeof Primitive.Root>((props, ref) => <Primitive.Root {...props} ref={ref} />),

  /**
   * An item in the group.
   */
  Item: createElement<typeof Primitive.Item>((props, ref) => (
    <Primitive.Item {...props} ref={ref} className={clsx("cursor-pointer", props.className)} />
  ))
};

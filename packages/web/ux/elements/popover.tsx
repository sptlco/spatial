// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-popover";

import { createElement } from "..";

/**
 * Displays rich content in a portal, triggered by a button.
 */
export const Popover = {
  /**
   * Contains all the parts of a popover.
   */
  Root: createElement<typeof Primitive.Root>((props, _) => {
    return <Primitive.Root {...props} />;
  })
};

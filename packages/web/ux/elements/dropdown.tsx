// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-dropdown-menu";

import { createElement } from "..";

/**
 * Displays a menu to the user — such as a set of actions or functions — triggered by a button.
 */
export const Dropdown = {
  /**
   * Contains all the parts of a dropdown menu.
   */
  Root: createElement<typeof Primitive.Root>((props, _) => {
    return <Primitive.Root {...props} />;
  })
};

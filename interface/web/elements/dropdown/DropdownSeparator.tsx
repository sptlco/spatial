// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-dropdown-menu";
import { Element, ElementProps, Node, Separator } from "..";

/**
 * Create a new dropdown separator element.
 * @param props Configurable options for the element.
 * @returns A dropdown separator element.
 */
export const DropdownSeparator: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Separator
      ref={props.ref}
      style={props.style}
      className={props.className}
    >
      <Separator />
    </Primitive.Separator>
  );
};

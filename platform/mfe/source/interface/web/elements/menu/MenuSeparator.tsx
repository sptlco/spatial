// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-menubar";
import { Element, ElementProps, Node, Separator } from "..";

/**
 * Create a new menu separator element.
 * @param props Configurable options for the element.
 * @returns A menu separator element.
 */
export const MenuSeparator: Element = (props: ElementProps): Node => {
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

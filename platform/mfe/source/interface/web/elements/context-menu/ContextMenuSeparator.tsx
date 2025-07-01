// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-context-menu";
import { Element, ElementProps, Node, Separator } from "..";

/**
 * Create a new context menu separator element.
 * @param props Configurable options for the element.
 * @returns A context menu separator element.
 */
export const ContextMenuSeparator: Element = (props: ElementProps): Node => {
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

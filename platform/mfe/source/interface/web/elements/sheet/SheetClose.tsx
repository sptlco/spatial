// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-dialog";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new sheet close element.
 * @param props Configurable options for the element.
 * @returns A sheet close element.
 */
export const SheetClose: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Close
      asChild
      ref={props.ref}
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

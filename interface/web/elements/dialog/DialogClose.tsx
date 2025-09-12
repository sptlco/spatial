// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-dialog";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new dialog close element.
 * @param props Configurable options for the element.
 * @returns A dialog close element.
 */
export const DialogClose: Element = (props: ElementProps): Node => {
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

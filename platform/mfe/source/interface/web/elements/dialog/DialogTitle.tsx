// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-dialog";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new dialog title element.
 * @param props Configurable options for the element.
 * @returns A dialog title element.
 */
export const DialogTitle: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Title
      ref={props.ref}
      style={props.style}
      className={clsx("text-l font-bold", props.className)}
      children={props.children}
    />
  );
};

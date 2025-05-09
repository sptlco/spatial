// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-dialog";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new sheet title element.
 * @param props Configurable options for the element.
 * @returns A sheet title element.
 */
export const SheetTitle: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Title
      ref={props.ref}
      style={props.style}
      className={clsx("text-l font-bold", props.className)}
      children={props.children}
    />
  );
};

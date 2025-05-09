// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-dialog";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new sheet description element.
 * @param props Configurable options for the element.
 * @returns A sheet description element.
 */
export const SheetDescription: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Description
      ref={props.ref}
      style={props.style}
      className={clsx("text-foreground-secondary", props.className)}
      children={props.children}
    />
  );
};

// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-dialog";
import { Element, Node, SheetTriggerProps } from "../..";

/**
 * Create a new sheet trigger element.
 * @param props Configurable options for the element.
 * @returns A sheet trigger element.
 */
export const SheetTrigger: Element<SheetTriggerProps> = (
  props: SheetTriggerProps,
): Node => {
  return (
    <Primitive.Trigger
      ref={props.ref}
      style={props.style}
      asChild={props.asChild}
      className={props.className}
      children={props.children}
    />
  );
};

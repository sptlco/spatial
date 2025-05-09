// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-dialog";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new dialog description element.
 * @param props Configurable options for the element.
 * @returns A dialog description element.
 */
export const DialogDescription: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Description
      ref={props.ref}
      style={props.style}
      className={clsx("text-foreground-secondary", props.className)}
      children={props.children}
    />
  );
};

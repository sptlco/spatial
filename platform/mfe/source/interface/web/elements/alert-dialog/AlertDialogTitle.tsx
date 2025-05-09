// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-alert-dialog";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new alert dialog title element.
 * @param props Configurable options for the element.
 * @returns An alert dialog title element.
 */
export const AlertDialogTitle: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Title
      ref={props.ref}
      style={props.style}
      className={clsx("text-l font-bold", props.className)}
      children={props.children}
    />
  );
};

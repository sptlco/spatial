// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-alert-dialog";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new alert dialog description element.
 * @param props Configurable options for the element.
 * @returns An alert dialog description element.
 */
export const AlertDialogDescription: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Description
      ref={props.ref}
      style={props.style}
      className={clsx("text-foreground-secondary", props.className)}
      children={props.children}
    />
  );
};

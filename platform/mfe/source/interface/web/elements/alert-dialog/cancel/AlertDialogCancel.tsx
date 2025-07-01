// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-alert-dialog";
import { AlertDialogCancelProps, Element, Node } from "../..";

/**
 * Create a new alert dialog cancel element.
 * @param props Configurable options for the element.
 * @returns An alert dialog cancel element.
 */
export const AlertDialogCancel: Element<AlertDialogCancelProps> = (
  props: AlertDialogCancelProps,
): Node => {
  return (
    <Primitive.Cancel
      ref={props.ref}
      asChild={props.asChild}
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

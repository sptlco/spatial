// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-alert-dialog";
import { AlertDialogProps, Element, Node } from "..";

/**
 * Create a new alert dialog element.
 * @param props Configurable options for the element.
 * @returns An alert dialog element.
 */
export const AlertDialog: Element<AlertDialogProps> = (
  props: AlertDialogProps,
): Node => {
  return (
    <Primitive.Root
      open={props.open}
      onOpenChange={props.onOpenChange}
      children={props.children}
    />
  );
};

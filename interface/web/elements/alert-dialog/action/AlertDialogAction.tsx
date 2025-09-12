// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-alert-dialog";
import { AlertDialogActionProps, Element, Node } from "../..";

/**
 * Create a new alert dialog action element.
 * @param props Configurable options for the element.
 * @returns An alert dialog action element.
 */
export const AlertDialogAction: Element<AlertDialogActionProps> = (
  props: AlertDialogActionProps,
): Node => {
  return (
    <Primitive.Action
      ref={props.ref}
      asChild={props.asChild}
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-alert-dialog";
import { AlertDialogTriggerProps, Element, Node } from "../..";

/**
 * Create a new alert dialog trigger element.
 * @param props Configurable options for the element.
 * @returns An alert dialog trigger element.
 */
export const AlertDialogTrigger: Element<AlertDialogTriggerProps> = (
  props: AlertDialogTriggerProps,
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

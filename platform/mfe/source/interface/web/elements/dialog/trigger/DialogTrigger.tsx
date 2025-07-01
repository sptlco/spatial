// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-dialog";
import { DialogTriggerProps, Element, Node } from "../..";

/**
 * Create a new dialog trigger element.
 * @param props Configurable options for the element.
 * @returns A dialog trigger element.
 */
export const DialogTrigger: Element<DialogTriggerProps> = (
  props: DialogTriggerProps,
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

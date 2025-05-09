// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-popover";
import { Element, Node, PopupTriggerProps } from "../..";
import clsx from "clsx";

/**
 * Create a new popup trigger element.
 * @param props Configurable options for the element.
 * @returns A popup trigger element.
 */
export const PopupTrigger: Element<PopupTriggerProps> = (
  props: PopupTriggerProps,
): Node => {
  return (
    <Primitive.Trigger
      ref={props.ref}
      asChild={props.asChild}
      style={props.style}
      className={clsx("h-fit w-fit", props.className)}
      children={props.children}
    />
  );
};

// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-tooltip";
import { Element, Node, TooltipTriggerProps } from "../..";
import clsx from "clsx";

/**
 * Create a new tooltip trigger element.
 * @param props Configurable options for the element.
 * @returns A tooltip trigger element.
 */
export const TooltipTrigger: Element<TooltipTriggerProps> = (
  props: TooltipTriggerProps,
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

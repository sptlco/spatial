// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-dropdown-menu";
import { DropdownTriggerProps, Element, Node } from "../..";
import clsx from "clsx";

/**
 * Create a new dropdown trigger element.
 * @param props Configurable options for the element.
 * @returns A dropdown trigger element.
 */
export const DropdownTrigger: Element<DropdownTriggerProps> = (
  props: DropdownTriggerProps,
): Node => {
  return (
    <Primitive.Trigger
      ref={props.ref}
      style={props.style}
      className={clsx("group/trigger", "h-fit w-fit", props.className)}
      asChild={props.asChild}
      children={props.children}
    />
  );
};

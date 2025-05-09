// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-hover-card";
import { Element, HoverCardTriggerProps, Node } from "../..";
import clsx from "clsx";

/**
 * Create a new hover card trigger element.
 * @param props Configurable options for the element.
 * @returns A hover card trigger element.
 */
export const HoverCardTrigger: Element<HoverCardTriggerProps> = (
  props: HoverCardTriggerProps,
): Node => {
  return (
    <Primitive.Trigger
      asChild={props.asChild}
      ref={props.ref}
      style={props.style}
      className={clsx("h-fit w-fit", props.className)}
      children={props.children}
    />
  );
};

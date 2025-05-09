// Copyright © Spatial. All rights reserved.

"use client";

import { Drawer as Primitive } from "vaul";
import { DrawerTriggerProps, Element, Node } from "../..";
import clsx from "clsx";

/**
 * Create a new drawer trigger element.
 * @param props Configurable options for the element.
 * @returns A drawer trigger element.
 */
export const DrawerTrigger: Element<DrawerTriggerProps> = (
  props: DrawerTriggerProps,
): Node => {
  return (
    <Primitive.Trigger
      ref={props.ref}
      style={props.style}
      className={clsx("h-fit w-fit", props.className)}
      asChild={props.asChild}
      children={props.children}
    />
  );
};

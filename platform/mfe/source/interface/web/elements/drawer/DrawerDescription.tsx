// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Drawer as Primitive } from "vaul";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new drawer description element.
 * @param props Configurable options for the element.
 * @returns A drawer description element.
 */
export const DrawerDescription: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Description
      ref={props.ref}
      style={props.style}
      className={clsx("text-foreground-secondary", props.className)}
      children={props.children}
    />
  );
};

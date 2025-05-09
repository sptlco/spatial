// Copyright © Spatial. All rights reserved.

"use client";

import { Drawer as Primitive } from "vaul";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new drawer title element.
 * @param props Configurable options for the element.
 * @returns A drawer title element.
 */
export const DrawerTitle: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Title
      ref={props.ref}
      style={props.style}
      className={clsx("text-l font-bold", props.className)}
      children={props.children}
    />
  );
};

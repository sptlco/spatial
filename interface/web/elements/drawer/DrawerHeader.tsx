// Copyright © Spatial Corporation. All rights reserved.

"use client";

import clsx from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new drawer header element.
 * @param props Configurable options for the element.
 * @returns A drawer header element.
 */
export const DrawerHeader: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx("flex flex-col", props.children)}
    />
  );
};

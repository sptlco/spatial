// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-menubar";
import { Element, ElementProps, Node } from "../..";
import clsx from "clsx";

/**
 * Create a new menu bar element.
 * @param props Configurable options for the element.
 * @returns A menu bar element.
 */
export const MenuBar: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Root
      ref={props.ref}
      style={props.style}
      className={clsx("space-x-1/4u flex items-center", props.className)}
      children={props.children}
    />
  );
};

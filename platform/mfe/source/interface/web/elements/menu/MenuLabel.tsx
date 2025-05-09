// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-menubar";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new menu label element.
 * @param props Configurable options for the element.
 * @returns A menu label element.
 */
export const MenuLabel: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Label
      ref={props.ref}
      style={props.style}
      className={clsx(
        "px-1/2u py-1/4u",
        "text-s text-foreground-tertiary flex font-bold",
        props.className,
      )}
      children={props.children}
    />
  );
};

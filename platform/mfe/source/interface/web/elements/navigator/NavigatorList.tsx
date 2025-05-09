// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-navigation-menu";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new navigator list element.
 * @param props Configurable options for the element.
 * @returns A navigator list element.
 */
export const NavigatorList: Element = (props: ElementProps): Node => {
  return (
    <Primitive.List
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx(
        "group",
        "list-none",
        "flex flex-1",
        "data-[orientation=horizontal]:space-x-3/2u",
        "data-[orientation=horizontal]:items-center",
        "data-[orientation=horizontal]:justify-center",
        "data-[orientation=vertical]:flex-col",
        "data-[orientation=vertical]:space-y-3/4u",
        "data-[orientation=vertical]:w-full",
        props.className,
      )}
    />
  );
};

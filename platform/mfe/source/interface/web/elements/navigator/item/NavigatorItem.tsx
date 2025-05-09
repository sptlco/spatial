// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";

import * as Primitive from "@radix-ui/react-navigation-menu";
import { Element, NavigatorItemProps, Node } from "../..";

/**
 * Create a new navigator item element.
 * @param props Configurable options for the element.
 * @returns A navigator item element.
 */
export const NavigatorItem: Element<NavigatorItemProps> = (
  props: NavigatorItemProps,
): Node => {
  return (
    <Primitive.Item
      ref={props.ref}
      asChild={props.asChild}
      value={props.value}
      style={props.style}
      className={clsx("group/item", props.className)}
      children={props.children}
    />
  );
};

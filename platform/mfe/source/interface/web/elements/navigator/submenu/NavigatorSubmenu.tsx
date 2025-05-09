// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-navigation-menu";
import { Element, NavigatorSubmenuProps, Node } from "../..";
import clsx from "clsx";

/**
 * Create a new navigator submenu element.
 * @param props Configurable options for the element.
 * @returns A navigator submenu element.
 */
export const NavigatorSubmenu: Element<NavigatorSubmenuProps> = (
  props: NavigatorSubmenuProps,
): Node => {
  return (
    <Primitive.Sub
      ref={props.ref}
      defaultValue={props.defaultValue}
      value={props.value}
      onValueChange={props.onChange}
      orientation="vertical"
      style={props.style}
      children={props.children}
      className={clsx("py-1/2u", props.className)}
    />
  );
};

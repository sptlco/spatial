// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-dropdown-menu";
import { Element, ElementProps, Node } from "../..";

/**
 * Create a new dropdown submenu element.
 * @param props Configurable options for the element.
 * @returns A dropdown submenu element.
 */
export const DropdownSubmenu: Element = (props: ElementProps): Node => {
  return <Primitive.Sub children={props.children} />;
};

// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-menubar";
import { Element, ElementProps, Node } from "../..";

/**
 * Create a new menu submenu element.
 * @param props Configurable options for the element.
 * @returns A menu submenu element.
 */
export const MenuSubmenu: Element = (props: ElementProps): Node => {
  return <Primitive.Sub children={props.children} />;
};

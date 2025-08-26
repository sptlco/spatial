// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-context-menu";
import { Element, ElementProps, Node } from "../..";

/**
 * Create a new context menu submenu element.
 * @param props Configurable options for the element.
 * @returns A context menu submenu element.
 */
export const ContextMenuSubmenu: Element = (props: ElementProps): Node => {
  return <Primitive.Sub children={props.children} />;
};

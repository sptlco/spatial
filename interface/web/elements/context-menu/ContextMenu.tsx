// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-context-menu";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new context menu element.
 * @param props Configurable options for the element.
 * @returns A context menu element.
 */
export const ContextMenu: Element = (props: ElementProps): Node => {
  return <Primitive.Root children={props.children} />;
};

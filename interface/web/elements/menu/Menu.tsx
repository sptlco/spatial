// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-menubar";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new menu element.
 * @param props Configurable options for the element.
 * @returns A menu element.
 */
export const Menu: Element = (props: ElementProps): Node => {
  return <Primitive.Menu children={props.children} />;
};

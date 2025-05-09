// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-dropdown-menu";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new dropdown element.
 * @param props Configurable options for the element.
 * @returns A dropdown element.
 */
export const Dropdown: Element = (props: ElementProps): Node => {
  return <Primitive.Root children={props.children} />;
};

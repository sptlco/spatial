// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-hover-card";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new hover card element.
 * @param props Configurable options for the element.
 * @returns A hover card element.
 */
export const HoverCard: Element = (props: ElementProps): Node => {
  return <Primitive.Root children={props.children} />;
};

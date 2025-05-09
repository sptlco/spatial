// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Preview as Primitive } from "@react-email/components";

import { Element, ElementProps, Node } from ".";

/**
 * Create a new preview element.
 * @param props Configurable options for the element.
 * @returns A preview element.
 */
export const Preview: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      style={props.style}
      className={clsx("hidden", props.className)}
      children={props.children as string}
    />
  );
};

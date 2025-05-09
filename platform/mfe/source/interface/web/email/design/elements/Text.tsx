// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Text as Primitive } from "@react-email/components";

import { Element, ElementProps, Node } from ".";

/**
 * Create a new text element.
 * @param props Configurable options for the element.
 * @returns A text element.
 */
export const Text: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      style={props.style}
      className={clsx("m-0 text-inherit", props.className)}
      children={props.children}
    />
  );
};

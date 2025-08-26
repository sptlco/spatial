// Copyright © Spatial Corporation. All rights reserved.

import { Heading as Primitive } from "@react-email/components";
import { Element, ElementProps, Node } from ".";

/**
 * Create a new heading element.
 * @param props Configurable options for the element.
 * @returns A heading element.
 */
export const Heading: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

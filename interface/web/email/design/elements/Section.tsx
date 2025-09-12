// Copyright © Spatial Corporation. All rights reserved.

import { Section as Primitive } from "@react-email/components";
import { Element, ElementProps, Node } from ".";

/**
 * Create a new section element.
 * @param props Configurable options for the element.
 * @returns A section element.
 */
export const Section: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

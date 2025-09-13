// Copyright © Spatial Corporation. All rights reserved.

import { Hr as Primitive } from "@react-email/components";
import { Element, ElementProps, Node } from ".";

/**
 * Create a new HR element.
 * @param props Configurable options for the element.
 * @returns A HR element.
 */
export const HR: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

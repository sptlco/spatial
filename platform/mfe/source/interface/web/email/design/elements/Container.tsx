// Copyright © Spatial Corporation. All rights reserved.

import { Container as Primitive } from "@react-email/components";
import { Element, ElementProps, Node } from ".";

/**
 * Create a new container element.
 * @param props Configurable options for the element.
 * @returns A container element.
 */
export const Container: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

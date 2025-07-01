// Copyright © Spatial Corporation. All rights reserved.

import { Row as Primitive } from "@react-email/components";
import { Element, ElementProps, Node } from ".";

/**
 * Create a new row element.
 * @param props Configurable options for the element.
 * @returns A row element.
 */
export const Row: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

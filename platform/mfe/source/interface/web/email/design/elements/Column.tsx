// Copyright © Spatial. All rights reserved.

import { Column as Primitive } from "@react-email/components";
import { Element, ElementProps, Node } from ".";

/**
 * Create a new column element.
 * @param props Configurable options for the element.
 * @returns A column element.
 */
export const Column: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

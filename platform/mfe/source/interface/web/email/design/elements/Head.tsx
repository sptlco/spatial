// Copyright © Spatial. All rights reserved.

import { Head as Primitive } from "@react-email/components";
import { Element, ElementProps, Node } from ".";

/**
 * Create a new head element.
 * @param props Configurable options for the element.
 * @returns A head element.
 */
export const Head: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

// Copyright © Spatial Corporation. All rights reserved.

import { Html as Primitive } from "@react-email/components";
import { Element, ElementProps, Node } from ".";

/**
 * Create a new HTML element.
 * @param props Configurable options for the element.
 * @returns An HTML element.
 */
export const Html: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      lang="en"
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

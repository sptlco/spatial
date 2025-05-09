// Copyright © Spatial. All rights reserved.

import { VisuallyHidden as Primitive } from "@radix-ui/react-visually-hidden";
import { Element, ElementProps, Node } from ".";

/**
 * Create a new hidden element.
 * @param props Configurable options for the element.
 * @returns A hidden element.
 */
export const Hidden: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      ref={props.ref}
      children={props.children}
      style={props.style}
      className={props.className}
    />
  );
};

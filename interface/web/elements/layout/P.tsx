// Copyright © Spatial Corporation. All rights reserved.

import { Element, ElementProps, Node } from "..";

/**
 * Create a new paragraph element.
 * @param props Configurable options for the element.
 * @returns A paragraph element.
 */
export const P: Element = (props: ElementProps): Node => {
  return (
    <p
      ref={props.ref}
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

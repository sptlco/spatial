// Copyright © Spatial Corporation. All rights reserved.

import { Element, ElementProps, Node } from "..";

/**
 * Create a new div element.
 * @param props Configurable options for the element.
 * @returns A div element.
 */
export const Div: Element = (props: ElementProps): Node => {
  return (
    <div
      ref={props.ref}
      role={props.role}
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

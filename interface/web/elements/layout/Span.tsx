// Copyright © Spatial Corporation. All rights reserved.

import { Element, ElementProps, Node } from "..";

/**
 * Create a new span element.
 * @param props Configurable options for the element.
 * @returns A span element.
 */
export const Span: Element = (props: ElementProps): Node => {
  return (
    <span
      ref={props.ref}
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

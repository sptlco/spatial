// Copyright © Spatial Corporation. All rights reserved.

import { Element, ElementProps, Node, Span } from "..";

/**
 * Create a new alert title element.
 * @param props Configurable options for the element.
 * @returns An alert title element.
 */
export const AlertTitle: Element = (props: ElementProps): Node => {
  return (
    <Span
      ref={props.ref}
      style={props.style}
      className="font-bold"
      children={props.children}
    />
  );
};

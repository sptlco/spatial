// Copyright © Spatial Corporation. All rights reserved.

import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new alert buttons element.
 * @param props Configurable options for the element.
 * @returns An alert buttons element.
 */
export const AlertButtons: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className="space-x-1/2u flex items-center"
      children={props.children}
    />
  );
};
